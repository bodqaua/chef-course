using Chef.Models;
using Chef.Models.Database;
using Chef.Models.Entities;
using Chef.Shared;
using Chef.Validators;
using Chef.Validators.InputValidators;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chef.ViewModels.Recipe
{
    /// <summary>
    /// Interaction logic for RecipeAdd.xaml
    /// </summary>
    public partial class RecipeAdd : Page
    {
        private readonly ValidationController validationController;
        private readonly ProductService productService;
        private readonly RecipeService recipeService;
        private readonly ViewModelFactory viewModelFactory;
        private FormGroup formGroup = new FormGroup();
        private List<Product> products;
        private List<ImageItem> images = new List<ImageItem>();
        private List<IngredientItem> ingredientList = new List<IngredientItem>();
        public RecipeAdd(ValidationController validationController,
                                     ProductService productService,
                                     ViewModelFactory viewModelFactory,
                                     RecipeService recipeService)
        {
            this.validationController = validationController;
            this.productService = productService;
            this.recipeService = recipeService;
            this.viewModelFactory = viewModelFactory;

            this.DataContext = this.formGroup;
            InitializeComponent();
            this.getIngredients();
        }

        private void getIngredients()
        {
            this.products = this.productService.loadProducts().ToList();
            IngredientsComboBox.SelectedValuePath = "Key";
            IngredientsComboBox.DisplayMemberPath = "Value";
            foreach (Product product in this.products)
            {
                IngredientsComboBox.Items.Add(new KeyValuePair<int, string>(product.Id, product.Name));
            }
        }

        private void addIngredient(object sender, EventArgs e)
        {
            if (IngredientsComboBox.SelectedItem == null)
            {
                return;
            }

            KeyValuePair<int, string> item = (KeyValuePair<int, string>)IngredientsComboBox.SelectedItem;
            List<IngredientItem> items = IngredientsList.Items.Cast<IngredientItem>().ToList();
            int index = items.FindIndex((product) => product.Id == item.Key);
            if (index != -1 || this.formGroup.IngredientQuantity.Value == "")
            {
                return;
            }

            IngredientItem ingredient = new IngredientItem
            {
                Id = item.Key,
                Name = item.Value,
                Quantity = Convert.ToDouble(this.formGroup.IngredientQuantity.Value),
                Action = true,
                ListId = Guid.NewGuid().ToString()
            };

            IngredientsList.Items.Add(ingredient);
            this.ingredientList.Add(ingredient);
            this.clearSelect();
        }

        private void DeleteItem(object sender, EventArgs e)
        {
            List<IngredientItem> items = IngredientsList.Items.Cast<IngredientItem>().ToList();
            Button element = (Button)sender;

            int index = items.FindIndex((item) => item.ListId == element.Tag.ToString());
            if (index == -1)
            {
                return;
            }
            IngredientsList.Items.RemoveAt(index);
            this.ingredientList.RemoveAt(index);

        }

        private void clearSelect()
        {
            IngredientsComboBox.SelectedIndex = -1;
        }

        private void OpenUploadImage(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".jpg";
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            fileDialog.CheckFileExists = true;
            fileDialog.Multiselect = false;
            bool result = Convert.ToBoolean(fileDialog.ShowDialog());

            if (result)
            {
                StreamConverter converter = new StreamConverter();
                byte[] data = converter.convertToByte(fileDialog.FileName);
                string base64 = Convert.ToBase64String(data);
                byte[] binaryData = Convert.FromBase64String(base64);
                ImageItem imageItem = ImageItem.CreateImageItem(base64);
                this.images.Add(imageItem);

                if (binaryData.Length == 0)
                {
                    MessageBox.Show("Поврежденное изображение");
                    return;
                }

                Image image = converter.base64ToImage(binaryData);
                image.Tag = imageItem.ListId;
                image.MouseDown += (sender, args) => { this.deleteImage(sender); };
                ImagePanel.Children.Add(image);
            }
        }

        private void deleteImage(object sender)
        {
            Image element = (Image)sender;
            int index = this.images.FindIndex((item) => item.ListId == Convert.ToString(element.Tag));
            if (index == -1)
            {
                return;
            }

            this.images.RemoveAt(index);
            ImagePanel.Children.Remove(element);
        }

        private void SaveRecipe(object sender, RoutedEventArgs e)
        {
            if (!this.validationController.isFormValid(this, this.formGroup.controls))
            {
                return;
            }

            RecipeEntity newRecipe = new RecipeEntity
            {
                Name = Convert.ToString(this.formGroup.Name.Value),
                Calories = Convert.ToInt32(this.formGroup.Calories.Value),
                Proteins = Convert.ToDouble(this.formGroup.Proteins.Value),
                Fats = Convert.ToDouble(this.formGroup.Fats.Value),
                Carbohydrate = Convert.ToDouble(this.formGroup.Carbohydrate.Value),
                Description = Convert.ToString(this.formGroup.Description.Value),
                Ingredients = IngredientItem.PrepareIngredients(this.ingredientList),
                Images = ImageItem.PrepareImages(this.images.Select(x => x.Value).ToList())
            };
            this.recipeService.createRecipe(newRecipe);
        }
    }

    public class IngredientItem : Ingredient
    {
        public string ListId { get; set; }
        public bool Action { get; set; }

        public static List<Ingredient> PrepareIngredients(List<IngredientItem> ingredients)
        {
            List<Ingredient> dbIngredients = new List<Ingredient>();
            foreach (IngredientItem item in ingredients)
            {
                dbIngredients.Add(
                    new Ingredient { Name = item.Name, Quantity = item.Quantity }
                );
            }
            return dbIngredients;
        }

    }

    public class ImageItem
    {
        public string ListId { get; set; }
        public string Value { get; set; }

        public static ImageItem CreateImageItem(string value)
        {
            return new ImageItem
            {
                ListId = Guid.NewGuid().ToString(),
                Value = value
            };
        }

        public static List<DbImage> PrepareImages(List<string> images)
        {
            List<DbImage> preparedImages = new List<DbImage>();

            for (int i = 0; i < images.Count; i++)
            {
                preparedImages.Add(new DbImage
                {
                    Id = i,
                    Content = images[i]
                });
            }

            return preparedImages;
        }

    }

    public class StreamConverter
    {
        private string trashIcon = "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";

        public byte[] convertToByte(string fileName)
        {
            Stream stream = File.OpenRead(fileName);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Flush();
            stream.Close();
            return data;
        }

        public byte[] convertBase64ToByte(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        public Image base64ToImage(byte[] binaryData)
        {
            BitmapImage imageData = new BitmapImage();
            imageData.BeginInit();
            imageData.StreamSource = new MemoryStream(binaryData);
            imageData.EndInit();

            BitmapImage trash = new BitmapImage();
            trash.BeginInit();
            trash.StreamSource = new MemoryStream(this.convertBase64ToByte(this.trashIcon));
            trash.EndInit();

            Image image = new Image();
            image.Source = imageData;
            image.Width = 40;
            image.Height = 40;
            image.Margin = new Thickness(12, 0, 0, 0);
            image.Stretch = Stretch.Fill;
            image.Cursor = Cursors.Hand;

            image.MouseEnter += (sender, args) => { image.Source = trash; };
            image.MouseLeave += (sender, args) => { image.Source = imageData; };

            return image;
        }
    }

    public class FormGroup
    {
        public ITextBoxGroup Name { get; set; } = new TextBoxGroup
        {
            Name = "Name",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator()
            }
        };

        public ITextBoxGroup Calories { get; set; } = new TextBoxGroup
        {
            Name = "Calories",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator(),
                new MinValidator(0)
            }
        };

        public ITextBoxGroup Proteins { get; set; } = new TextBoxGroup
        {
            Name = "Proteins",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator(),
                new MinValidator(0)
            }
        };

        public ITextBoxGroup Fats { get; set; } = new TextBoxGroup
        {
            Name = "Fats",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator(),
                new MinValidator(0)
            }
        };

        public ITextBoxGroup Carbohydrate { get; set; } = new TextBoxGroup
        {
            Name = "Carbohydrate",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator(),
                new MinValidator(0),
            }
        };

        public ITextBoxGroup Description { get; set; } = new TextBoxGroup
        {
            Name = "Description",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator()
            }
        };

        public ITextBoxGroup IngredientQuantity { get; set; } = new TextBoxGroup
        {
            Name = "IngredientQuantity",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator(),
                new MinValidator(0)
            }
        };

        public List<ITextBoxGroup> controls = new List<ITextBoxGroup>();

        public FormGroup()
        {
            this.controls.Add(this.Name);
            this.controls.Add(this.Calories);
            this.controls.Add(this.Proteins);
            this.controls.Add(this.Fats);
            this.controls.Add(this.Carbohydrate);
            this.controls.Add(this.Description);
            this.controls.Add(this.IngredientQuantity);
        }
    }
}

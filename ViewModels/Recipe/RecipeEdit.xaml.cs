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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chef.ViewModels.Recipe
{
    /// <summary>
    /// Interaction logic for RecipeEdit.xaml
    /// </summary>
    public partial class RecipeEdit : AbstractPageController
    {
        private readonly int recipeId;
        private readonly RecipeEntity recipe;
        private readonly RecipeService recipeService;
        private readonly ViewModelFactory viewModelFactory;
        private readonly ProductService productService;
        private FormGroup formGroup;
        private List<IngredientItem> ingredients = new List<IngredientItem>();
        private List<Product> products = new List<Product>();
        private List<ImageItem> images = new List<ImageItem>();
        public RecipeEdit(int recipeId,
                          RecipeService recipeService,
                          ProductService productService,
                          ViewModelFactory viewModelFactory)
        {
            this.recipeId = recipeId;
            this.recipeService = recipeService;
            this.viewModelFactory = viewModelFactory;
            this.productService = productService;
            this.recipe = this.recipeService.getRecipeById(recipeId);

            this.formGroup = new FormGroup(this.recipe);
            this.DataContext = this.formGroup;
            this.Init(this);
            InitializeComponent();
            this.InitEdit();
            this.getIngredients();
            this.PrepareImages();
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

        private void InitEdit()
        {
            foreach(Ingredient item in recipe.Ingredients)
            {
                IngredientItem newItem = new IngredientItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Action = true,
                    ListId = Guid.NewGuid().ToString()
                };
                ingredients.Add(newItem);
                IngredientsList.Items.Add(newItem);
            }
        }

        private void PrepareImages()
        {
            foreach(DbImage dbImage in this.recipe.Images)
            {
                StreamConverter converter = new StreamConverter();
                byte[] binaryData = Convert.FromBase64String(dbImage.Content);
                ImageItem imageItem = ImageItem.CreateImageItemWithId(dbImage.Id ,dbImage.Content);
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

        private void AddIngredient(object sender, RoutedEventArgs e)
        {
            if (IngredientsComboBox.SelectedItem == null)
            {
                return;
            }

            KeyValuePair<int, string> item = (KeyValuePair<int, string>)IngredientsComboBox.SelectedItem;
            List<IngredientItem> items = IngredientsList.Items.Cast<IngredientItem>().ToList();
            int index = items.FindIndex((product) => product.Id == item.Key);
            if (index != -1 || Convert.ToString(this.formGroup.IngredientQuantity.Value) == "")
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
            this.ingredients.Add(ingredient);
            this.clearSelect();
        }

        private void DeleteIngredient(object sender, RoutedEventArgs e)
        {
            List<IngredientItem> items = IngredientsList.Items.Cast<IngredientItem>().ToList();
            Button element = (Button)sender;

            int index = items.FindIndex((item) => item.ListId == element.Tag.ToString());
            if (index == -1)
            {
                return;
            }
            IngredientsList.Items.RemoveAt(index);
            this.ingredients.RemoveAt(index);
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            RecipeEntity entity = new RecipeEntity
            {
                Id = this.recipe.Id,
                Name = Convert.ToString(this.formGroup.Name.Value),
                Calories = Convert.ToInt32(this.formGroup.Calories.Value),
                Proteins = Convert.ToDouble(this.formGroup.Proteins.Value),
                Fats = Convert.ToDouble(this.formGroup.Fats.Value),
                Carbohydrate = Convert.ToDouble(this.formGroup.Carbohydrate.Value),
                Description = Convert.ToString(this.formGroup.Description.Value),
                Ingredients = IngredientItem.PrepareIngredients(this.ingredients),
                Images = ImageItem.PrepareImagesWithId(this.images)
            };
            this.recipeService.UpdateRecipe(entity);
            this.Redirect(this.viewModelFactory.createRecipeListPage());
        }

        private void clearSelect()
        {
            IngredientsComboBox.SelectedIndex = -1;
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

            public FormGroup(RecipeEntity recipe)
            {
                this.Name.Value = recipe.Name;
                this.Calories.Value = recipe.Calories;
                this.Proteins.Value = recipe.Proteins;
                this.Fats.Value = recipe.Fats;
                this.Carbohydrate.Value = recipe.Carbohydrate;
                this.Description.Value = recipe.Description;

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
}

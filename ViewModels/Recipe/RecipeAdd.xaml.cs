using Chef.Models;
using Chef.Models.Database;
using Chef.Shared;
using Chef.Validators;
using Chef.Validators.InputValidators;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chef.ViewModels.Recipe
{
    /// <summary>
    /// Interaction logic for RecipeAdd.xaml
    /// </summary>
    public partial class RecipeAdd : AbstractPageController
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
            this.Init(this);
            InitializeComponent();
            this.InitBackNavigation(this.viewModelFactory.createRecipeListPage(), BackPanel);
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
            this.Redirect(this.viewModelFactory.createRecipeListPage());
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
                new RequiredValidator(),
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

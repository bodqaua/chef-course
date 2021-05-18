using Chef.Models;
using Chef.Models.Database;
using Chef.Models.Entities;
using Chef.Shared;
using Chef.Validators;
using Chef.Validators.InputValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chef.ViewModels.Pages
{
    /// <summary>
    /// Interaction logic for RecipeDayCounter.xaml
    /// </summary>
    public partial class RecipeDayCounter : AbstractPageController
    {
        private readonly ViewModelFactory viewModelFactory;
        private readonly RecipeService recipeService;
        private readonly ProductService productService;
        private readonly ValidationController validationController;
        private readonly IngredientData ingredientData;

        private List<RecipeEntity> recipes;
        public RecipeDayCounter(
            RecipeService recipeService,
            ProductService productService,
            ViewModelFactory viewModelFactory,
            ValidationController validationController)
        {
            this.recipeService = recipeService;
            this.productService = productService;
            this.viewModelFactory = viewModelFactory;
            this.validationController = validationController;
            InitializeComponent();
            this.ingredientData = new IngredientData(this.recipeService, this.productService, IngredientsGrid, TotalCalories, TotalFats, TotalProteins, TotalCarbonohydrates);
            this.DataContext = ingredientData;
            this.Init(this);
            this.InitBackNavigation(this.viewModelFactory.createHomePage(), BackPanel);
            this.LoadRecipes();
        }

        private void LoadRecipes()
        {
            this.recipes = this.recipeService.loadRecipes();
            this.InitRecipes();
        }

        private void InitRecipes()
        {
            RenderComboboxItems(BreakfastSelector, this.recipes);
            RenderComboboxItems(LunchSelector, this.recipes);
            RenderComboboxItems(DinnerSelector, this.recipes);
        }

        private static void RenderComboboxItems(ComboBox comboBox, List<RecipeEntity> items)
        {
            comboBox.SelectedValuePath = "Key";
            comboBox.DisplayMemberPath = "Value";
            foreach (RecipeEntity item in items)
            {
                comboBox.Items.Add(new KeyValuePair<int, string>(item.Id, item.Name));
            }
        }

        private void SaveDayRecipes(object sender, RoutedEventArgs e)
        {
            if (!this.validationController.isFormValid(this, this.ingredientData.controls))
            {
                return;
            }

            if (!this.ingredientData.IsAllRecipesSelected())
            {
                MessageBox.Show("Выберите все блюда!");
                return;
            }


            List<Product> products =  this.ingredientData.GetProductsToFetchWarehouse();
            bool isStocksCorrect = this.productService.CheckStocks(products);
            if (!isStocksCorrect)
            {
                MessageBox.Show("Недостаточно запасов, пополните на складе!");
                return;
            }
            this.productService.SubstractProducts(products);
            MessageBox.Show("Склад обновлен");
            this.Redirect(this.viewModelFactory.createHomePage());
        }

        public class IngredientData
        {
            public double TotalCalories { get; set; }
            public double TotalFats { get; set; }
            public double TotalProteins { get; set; }
            public double TotalCarbonohydrates { get; set; }

            public List<ITextBoxGroup> controls = new List<ITextBoxGroup>();
            private RecipeEntity BreakfastEntity;
            private RecipeEntity LunchEntity;
            private RecipeEntity DinnerEntity;

            private readonly TextBlock CaloriesElement;
            private readonly TextBlock FatsElement;
            private readonly TextBlock ProteinsElement;
            private readonly TextBlock CarbonohydratesElement;
            private readonly DataGrid IngredientsGrid;

            private int _Breakfast;
            private int _Lunch;
            private int _Dinner;

            private readonly RecipeService recipeService;
            private readonly ProductService productService;
            public IngredientData(
                RecipeService recipeService,
                ProductService productService,
                DataGrid IngredientsGrid,
                TextBlock CaloriesElement,
                TextBlock FatsElement,
                TextBlock ProteinsElement,
                TextBlock CarbonohydratesElement)
            {
                this.recipeService = recipeService;
                this.productService = productService;

                this.IngredientsGrid = IngredientsGrid;
                this.CaloriesElement = CaloriesElement;
                this.FatsElement = FatsElement;
                this.ProteinsElement = ProteinsElement;
                this.CarbonohydratesElement = CarbonohydratesElement;


                this.SetElementText(this.CaloriesElement, 0);
                this.SetElementText(this.FatsElement, 0);
                this.SetElementText(this.ProteinsElement, 0);
                this.SetElementText(this.CarbonohydratesElement, 0);

                this.controls.Add(this.PeopleCount);
            }

            public int Breakfast
            {
                get => _Breakfast;
                set
                {
                    _Breakfast = value;
                    this.BreakfastEntity = this.GetEntityById(value);
                    this.CalculateNutrition();
                }
            }


            public int Lunch
            {
                get => _Lunch;
                set
                {
                    _Lunch = value;
                    this.LunchEntity = this.GetEntityById(value);
                    this.CalculateNutrition();
                }
            }

            public int Dinner
            {
                get => _Dinner;
                set
                {
                    _Dinner = value;
                    this.DinnerEntity = this.GetEntityById(value);
                    this.CalculateNutrition();
                }
            }


            private void CalculateNutrition()
            {
                this.ClearData();
                this.CountRecipe(this.BreakfastEntity);
                this.CountRecipe(this.LunchEntity);
                this.CountRecipe(this.DinnerEntity);

                this.SetElementText(this.CaloriesElement, this.TotalCalories);
                this.SetElementText(this.FatsElement, this.TotalFats);
                this.SetElementText(this.ProteinsElement, this.TotalProteins);
                this.SetElementText(this.CarbonohydratesElement, this.TotalCarbonohydrates);
            }

            private RecipeEntity GetEntityById(int id)
            {
                return this.recipeService.getRecipeById(id);
            }

            private void ClearData()
            {
                this.TotalCalories = 0;
                this.TotalFats = 0;
                this.TotalProteins = 0;
                this.TotalCarbonohydrates = 0;

                this.IngredientsGrid.Items.Clear();
            }

            private void CountRecipe(RecipeEntity recipe)
            {
                if (recipe == null) return;

                this.TotalCalories += recipe.Calories;
                this.TotalFats += recipe.Fats;
                this.TotalProteins += recipe.Proteins;
                this.TotalCarbonohydrates += recipe.Carbohydrate;

                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    Product product = this.productService.GetProductByName(ingredient.Name);
                    if (product != null)
                    {
                        product.Quantity = ingredient.Quantity * Convert.ToInt32(this.PeopleCount.Value);
                        this.IngredientsGrid.Items.Add(product);
                    }
                }
            }

            private void SetElementText(TextBlock element, double value)
            {
                element.Text = Math.Round(value, 2).ToString();
            }

            public List<Product> GetProductsToFetchWarehouse()
            {
                List<Product> products = new List<Product>();
                foreach(Product item in this.IngredientsGrid.Items)
                {
                    products.Add(item);
                }
                return products;
            }

            public bool IsAllRecipesSelected()
            {
                this.CalculateNutrition();
                return this.DinnerEntity != null && this.LunchEntity != null && this.BreakfastEntity != null;
            }

            public ITextBoxGroup PeopleCount { get; set; } = new TextBoxGroup
            {
                Name = "PeopleCount",
                Value = "1",
                Validators = new List<AbstractValidator>()
                {
                    new RequiredValidator(),
                    new MinValidator(1),
                    new MaxValidator(100)
                }
            };
        }
    }
}

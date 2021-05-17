using Chef.Models;
using Chef.Models.Database;
using Chef.Shared;
using System.Collections.Generic;
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

        private List<RecipeEntity> recipes;
        public RecipeDayCounter(
            RecipeService recipeService,
            ProductService productService,
            ViewModelFactory viewModelFactory)
        {
            this.recipeService = recipeService;
            this.productService = productService;
            this.viewModelFactory = viewModelFactory;
            InitializeComponent();
            this.Init(this);
            this.InitBackNavigation(this.viewModelFactory.createHomePage(), BackPanel);
            this.LoadRecipes();
        }

        private void LoadRecipes()
        {
            this.recipes = this.recipeService.loadRecipes();
            this.InitRecipes();
        }

        private void InitRecipes() {
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

        public class IngredientData
        {
            public RecipeEntity Breakfast { get; set; }
            public RecipeEntity Lunch { get; set; }



            private RecipeEntity _dinner;
            public RecipeEntity Dinner { 
                get => _dinner; 
                set {
                    MessageBox.Show("Data change");
                    _dinner = value;
                }
            }

            public double TotalCalories = 0;
            public double TotalFats = 0;
            public double TotalProteinst = 0;
            public double TotalCarbonohydrates = 0;
            private object MessagBox;
        }
    }
}

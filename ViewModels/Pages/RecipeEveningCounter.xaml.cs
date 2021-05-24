using Chef.Models;
using Chef.Models.Database;
using Chef.Models.Entities;
using Chef.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chef.ViewModels.Pages
{
    /// <summary>
    /// Interaction logic for RecipeEveningCounter.xaml
    /// </summary>
    public partial class RecipeEveningCounter : AbstractPageController
    {
        private readonly ViewModelFactory viewModelFactory;
        private readonly RecipeService recipeService;
        private readonly ProductService productService;
        private readonly ValidationController validationController;
        private List<RecipeEntity> recipes;
        private List<ProductItem> products = new List<ProductItem>();
        private List<RecipeEntityItem> recipesList = new List<RecipeEntityItem>();

        public RecipeEveningCounter(
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
            this.Init(this);
            this.InitBackNavigation(this.viewModelFactory.createHomePage(), BackPanel);

            this.InitRecipes();
        }

        private void InitRecipes()
        {
            this.recipes = this.recipeService.loadRecipes();
            RecipesSelector.SelectedValuePath = "Id";
            RecipesSelector.DisplayMemberPath = "Name";
            foreach (RecipeEntity recipe in recipes)
            {
                RecipesSelector.Items.Add(recipe);
            }
        }

        private void RecipesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox element = (ComboBox)sender;
            RecipeEntity selectedRecipeTemp = (RecipeEntity)element.SelectedItem;
            RecipeEntityItem selectedRecipe = this.CreateRecipeItemForList(selectedRecipeTemp);
            if (selectedRecipeTemp == null)
            {
                return;
            }
            List<Ingredient> ingredients = selectedRecipe.Recipe.Ingredients;
            RecipeList.Items.Add((selectedRecipe));
            this.recipesList.Add(selectedRecipe);
            foreach (Ingredient ingredient in ingredients)
            {
                Product productTemp = this.productService.GetProductByName(ingredient.Name);
                productTemp.Quantity = ingredient.Quantity;
                ProductItem product = this.CreateProductItem(productTemp, selectedRecipe.ListId);
                IngredientsGrid.Items.Add(product);
                this.products.Add(product);
            }
            this.ClearSelect();
        }

        private void ClearSelect()
        {
            RecipesSelector.SelectedIndex = -1;
        }

        private RecipeEntityItem CreateRecipeItemForList(RecipeEntity recipe)
        {
            return new RecipeEntityItem
            {
                ListId = Guid.NewGuid().ToString(),
                Recipe = recipe
            };
        }

        private ProductItem CreateProductItem(Product product, string ListId)
        {
            return new ProductItem
            {
                ListId = ListId,
                Product = product
            };
        }

        private void DeleteRecipe(object sender, RoutedEventArgs e)
        {
            Button element = (Button)sender;
            string ListId = element.Tag.ToString();
            List<RecipeEntityItem> items = RecipeList.Items.Cast<RecipeEntityItem>().ToList();

            this.products.RemoveAll(t => t.ListId == ListId);

            foreach(RecipeEntityItem item in RecipeList.Items)
            {
                if (item.ListId == ListId)
                {
                    RecipeList.Items.Remove(item);
                    this.recipesList.Remove(item);
                    break;
                }
            }

            List<ProductItem> ItemsToDelete = new List<ProductItem>();
            foreach (ProductItem item in IngredientsGrid.Items)
            {
                if (item.ListId == ListId)
                {
                    ItemsToDelete.Add(item);
                }
            }

            foreach(ProductItem item in ItemsToDelete)
            {
                this.products.Remove(item);
                IngredientsGrid.Items.Remove(item);
            }
        }

        private void saveHandler_Click(object sender, RoutedEventArgs e)
        {
            if (this.recipesList.Count == 0)
            {
                MessageBox.Show("Блюда не выбраны");
                return;
            }

            List<Product> productsToSpend = this.products.Select(t => t.Product).ToList();
            bool isStocksCorrect = this.productService.CheckStocks(productsToSpend);
            if (!isStocksCorrect)
            {
                MessageBox.Show("Недостаточно запасов, пополните на складе!");
                return;
            }
            this.productService.SubstractProducts(productsToSpend);
            MessageBox.Show("Склад обновлен");
            this.Redirect(this.viewModelFactory.createHomePage());
        }
    }

    class RecipeEntityItem
    {
        public string ListId { get; set; }
        public RecipeEntity Recipe { get; set; }
    }

    class ProductItem
    {
        public string ListId { get; set; }
        public Product Product { get; set; }
    }
}

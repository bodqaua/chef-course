using Chef.Models;
using Chef.Models.Database;
using Chef.Models.Entities;
using Chef.Shared;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Chef.ViewModels.Recipe
{
    /// <summary>
    /// Interaction logic for RecipeView.xaml
    /// </summary>
    public partial class RecipeView : AbstractPageController
    {
        private readonly int recipeId;
        private readonly RecipeService recipeService;
        private readonly ViewModelFactory viewModelFactory;

        private RecipeEntity recipe;
        public RecipeView(int recipeId,
                          RecipeService recipeService,
                          ViewModelFactory viewModelFactory)
        {
            this.recipeId = recipeId;
            this.recipeService = recipeService;
            this.viewModelFactory = viewModelFactory;

            this.Init(this);
            InitializeComponent();
            this.InitBackNavigation(this.viewModelFactory.createRecipeListPage(), BackPanel);
            this.LoadRecipe();
        }

        private void LoadRecipe()
        {
            this.recipe = this.recipeService.getRecipeById(this.recipeId);
            this.CheckRecipe();
            this.DataContext = new DataContext(this.recipe, SliderWrapper, IngredientList);
        }

        private void CheckRecipe()
        {
            if (this.recipe == null) this.GoBack();
        }

        private void GoBack()
        {
            this.Redirect(this.viewModelFactory.createRecipeListPage());
        }

        private void deleteRecipe(object sender, RoutedEventArgs e)
        {
            bool confirmation = this.GetConfirmation("Удалить рецепт?", "Внимание", MessageBoxButton.YesNo);
            if (confirmation)
            {
                this.recipeService.deleteRecipe(this.recipeId);
                this.GoBack();
            }
        }
    }

    public class DataContext: INotifyPropertyChanged
    {
        public string _ViewImage;
        public string ViewImage
        {
            get { return _ViewImage; }
            set
            {
                if (_ViewImage != value)
                {
                    _ViewImage = value;
                    OnPropertyChanged("ViewImage");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public RecipeEntity Recipe { get; set; }
        public readonly DockPanel SliderWrapper;
        public readonly ListView IngredientList;

        public DependencyProperty CursorProperty { get; private set; }

        public DataContext(RecipeEntity Recipe, DockPanel SliderWrapper, ListView IngredientList)
        {
            this.Recipe = Recipe;
            this.SliderWrapper = SliderWrapper;
            this.IngredientList = IngredientList;
            this.CheckImages();
            this.RenderImages();
            this.RenderIngredients();
        }

        private void CheckImages()
        {
            if (this.Recipe.Images.Count == 0)
            {
                this.ViewImage = null;
            }
            else
            {
                this.ViewImage = this.Recipe.Images[0].Content;
            }
        }

        public void SetImage(int index)
        {
            this.ViewImage = this.Recipe.Images[index].Content;
        }

        private void RenderImages()
        {
            for(int i = 0; i < this.Recipe.Images.Count; i++)
            {
                Image image = new Image();
                image.Source = this.PrepareImage(this.Recipe.Images[i].Content);
                image.Margin = new Thickness(6, 0, 6, 0);
                image.Width = 120;
                image.Cursor = Cursors.Hand;
                image.Tag = i;
                image.MouseDown += (sender, args) => {
                    Image img = (Image)sender;
                    int index = Convert.ToInt32(img.Tag);
                    this.SetImage(index);
                };
                this.SliderWrapper.Children.Add(image);
            }
        }

        private BitmapImage PrepareImage(string base64)
        {
            byte[] binaryData = Convert.FromBase64String(base64);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(binaryData);
            image.EndInit();
            return image;
        }

        private void RenderIngredients()
        {
            IngredientList.ItemsSource = this.Recipe.Ingredients;
        }
    }
}

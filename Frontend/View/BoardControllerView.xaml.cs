using Frontend.Model;
using Frontend.ViewModel;
using IntroSE.Kanban.Frontend.View;
using System.Windows;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardControllerView.xaml
    /// </summary>
    public partial class BoardControllerView : Window
    {
        private BoardControllerViewModel viewModel;


        public BoardControllerView(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardControllerViewModel(u);
            this.DataContext = viewModel;
        }

        /// <summary>
        /// Choose board button
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        /// <returns></returns>
        private void Choose_Button(object sender, RoutedEventArgs e)
        {
            BoardModel board = viewModel.ChooseBoard();
            if (board != null)
            {
                BoardView boardView = new BoardView(board);
                boardView.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Log Out button
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        /// <returns></returns>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

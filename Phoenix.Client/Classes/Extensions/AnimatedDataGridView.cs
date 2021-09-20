using System.Windows.Forms;

namespace Phoenix.Client.Classes.Extensions
{
    public class AnimatedDataGridView : DataGridView
    {
        private DataGridViewAnimator _imageAnimator;

        public AnimatedDataGridView()
            : base()
        {
            _imageAnimator = new DataGridViewAnimator(this);
        }
    }
}

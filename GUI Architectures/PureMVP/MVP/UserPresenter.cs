using System;
using System.Reflection;

namespace Library
{
    /// <summary>
    /// Summary description for UserPresenter.
    /// </summary>
    public class UserPresenter
    {

        IUserModel _model;
        IUserView _view;


        public UserPresenter(IUserModel model, IUserView view)
        {
            this._model = model;

            this._view = view;

            this.SetViewPropertiesFromModel();

            this.WireUpViewEvents();
        }

        private void WireUpViewEvents()
        {
            this._view.DataChanged += new EventHandler(_view_DataChanged);
            this._view.Save += new EventHandler(_view_Save);
        }

        private void SetModelPropertiesFromView()
        {
            foreach (PropertyInfo viewProperty in this._view.GetType().GetProperties())
            {
                if (viewProperty.CanRead)
                {
                    PropertyInfo modelProperty = this._model.GetType().GetProperty(viewProperty.Name);

                    if (modelProperty != null && modelProperty.PropertyType.Equals(viewProperty.PropertyType))
                    {
                        object valueToAssign = Convert.ChangeType(viewProperty.GetValue(this._view, null), modelProperty.PropertyType);

                        if (valueToAssign != null)
                        {
                            modelProperty.SetValue(this._model, valueToAssign, null);
                        }
                    }
                }
            }
        }

        private void SetViewPropertiesFromModel()
        {
            foreach (PropertyInfo viewProperty in this._view.GetType().GetProperties())
            {
                if (viewProperty.CanWrite)
                {
                    PropertyInfo modelProperty = this._model.GetType().GetProperty(viewProperty.Name);

                    if (modelProperty != null && modelProperty.PropertyType.Equals(viewProperty.PropertyType))
                    {
                        object modelValue = modelProperty.GetValue(this._model, null);

                        if (modelValue != null)
                        {
                            object valueToAssign = Convert.ChangeType(modelValue, viewProperty.PropertyType);

                            if (valueToAssign != null)
                            {
                                viewProperty.SetValue(this._view, valueToAssign, null);
                            }
                        }
                    }
                }
            }
        }

        private void _view_DataChanged(object sender, EventArgs e)
        {
            this.SetModelPropertiesFromView();
        }

        private bool Save()
        {
            return true;
        }

        private void _view_Save(object sender, EventArgs e)
        {
            this.Save();
        }
    }
}

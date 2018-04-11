using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DemoApplication.Attributes;

namespace DemoApplication
{
    public partial class Create : Page
    {
        private const BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
        private string _entityType = String.Empty;
        private List<TextBox> _txtControls = new List<TextBox>();
        private Type _typeEntity;
        private Type _typeRepository;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["entityType"]))
            {
                FindType();
                CreateControls();
            }
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["entityType"]))
                {
                    DisplayMessage(null);
                }
                else
                {
                    LoadDatainGrid();
                }
            }
        }

        private void CreateControls()
        {
            //Get All public property of current Class
            var entityProps = _typeEntity.GetProperties(flag);
            foreach (var prop in entityProps)
            {
                //Skipping if current property is Identity Field in table Column
                if (prop.GetCustomAttributes(typeof (IdentityAttribute), false).Length > 0)
                    continue;

                var controls = new TextBox();
                controls.ID = prop.Name;
                controls.Attributes.Add("class", "form-control");
                controls.Attributes.Add("placeholder", prop.Name);
                if (prop.Name == "GUID")
                {
                    controls.Text = Guid.NewGuid().ToString();
                    controls.Enabled = false;
                }
                PlaceHolder1.Controls.Add(controls);
                //txtControls.Add(controls);
            }
            var btn = new Button {ID = "btnSave", Text = "Save"};
            btn.Click += SaveEnity;
            btn.Attributes.Add("class", "btn btn-primary");
            PlaceHolder1.Controls.Add(btn);

            var btnUpdate = new Button {ID = "btnUpdate", Text = "Update", Visible = false};
            btnUpdate.Attributes.Add("class", "btn btn-primary");
            btnUpdate.Click += UpdateEnity;
            PlaceHolder1.Controls.Add(btnUpdate);

            var btnDelete = new Button {Visible = false, ID = "btnDelete", Text = "Delete"};
            btnDelete.Click += DeleteEnity;
            btnDelete.Attributes.Add("class", "btn btn-danger");
            PlaceHolder1.Controls.Add(btnDelete);

            var hdnField = new HiddenField {ID = "hdnFieldForID"};
            PlaceHolder1.Controls.Add(hdnField);
            //ViewState["allTxt"] = txtControls;
        }

        private void FindType()
        {
            var entity = Request.QueryString["entityType"];
            try
            {
                _typeRepository = Type.GetType("DemoApplication.Repository." + entity + "Repository", true, true);
                _typeEntity = Type.GetType("DemoApplication.Entities." + entity, true, true);

                //var obj = Convert.ChangeType(data, typeEntity);
            }
            catch
            {
                DisplayMessage(String.Format("This Entity ({0}) does not exist in current Assembly", entity));
            }
        }

        private void DisplayMessage(string msg)
        {
            if (!String.IsNullOrEmpty(msg))
                divErrMsg.InnerText = msg;

            panelWithoutEntity.Visible = true;
            panelWithEntity.Visible = false;
        }

        protected void SaveEnity(object sender, EventArgs e)
        {
            //Creating an instance of typeEntity
            var objEntity = Activator.CreateInstance(_typeEntity);
            var allControls = PlaceHolder1.Controls;
            var entityProps = _typeEntity.GetProperties(flag);
            foreach (var control in allControls.Cast<object>().Where(control => control.GetType() == typeof (TextBox)))
            {
                var control1 = control;
                foreach (var props in entityProps.Where(props => props.Name == ((TextBox) control1).ID))
                {
                    //Setting value in instance property
                    props.SetValue(objEntity, ((TextBox) control).Text, null);
                    break;
                }
            }
            //Instance of Repository
            var objRepo = Activator.CreateInstance(_typeRepository);
            var allMethods = objRepo.GetType().GetMethods();
            foreach (var method in allMethods.Where(method => method.Name == "Add"))
            {
                //Calling the Method of repository instance
                method.Invoke(objRepo, new[] {objEntity});
                break;
            }
        }

        private void LoadDatainGrid()
        {
            var objRepo = Activator.CreateInstance(_typeRepository);
            var method = objRepo.GetType().GetMethod("GetAll");
            var result = method.Invoke(objRepo, null);
            gridList.DataSource = result;
            gridList.DataBind();
        }

        protected void gridList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var guid = gridList.SelectedValue.ToString();
            var objrepo = Activator.CreateInstance(_typeRepository);
            var method = objrepo.GetType().GetMethod("GetByGUID");
            var result = method.Invoke(objrepo, new object[] {guid});
            PutResultInTextBox(result);
        }

        private void PutResultInTextBox(object result)
        {
            var allControls = PlaceHolder1.Controls;
            foreach (var control in allControls)
            {
                if (control is TextBox)
                {
                    var props = result.GetType().GetProperty((control as TextBox).ID);
                    (control as TextBox).Text = props.GetValue(result, null).ToString();
                }
                else if (control is Button)
                {
                    switch ((control as Button).ID)
                    {
                        case "btnDelete":
                            (control as Button).Visible = true;
                            break;
                        case "btnUpdate":
                            (control as Button).Visible = true;
                            break;
                    }
                }
                else if (control is HiddenField && (control as HiddenField).ID == "hdnFieldForID")
                {
                    var props = result.GetType().GetProperty("ID");
                    (control as HiddenField).Value = props.GetValue(result, null).ToString();
                }
            }
        }

        protected void UpdateEnity(object sender, EventArgs e)
        {
            var objEntity = Activator.CreateInstance(_typeEntity);
            var allControls = PlaceHolder1.Controls;
            var entityProps = _typeEntity.GetProperties(flag);
            foreach (var control in allControls)
            {
                if (control.GetType() == typeof (TextBox))
                {
                    var control1 = control;
                    foreach (var props in entityProps.Where(props => props.Name == ((TextBox) control1).ID))
                    {
                        props.SetValue(objEntity, ((TextBox) control).Text, null);
                        break;
                    }
                }
                else if (control.GetType() == typeof (HiddenField))
                {
                    foreach (var props in entityProps.Where(props => props.Name == "ID"))
                    {
                        props.SetValue(objEntity, Convert.ToInt32(((HiddenField) control).Value), null);
                        break;
                    }
                }
            }

            var objRepo = Activator.CreateInstance(_typeRepository);

            var allMethods = objRepo.GetType().GetMethods();
            foreach (var method in allMethods.Where(method => method.Name == "Update"))
            {
                method.Invoke(objRepo, new[] {objEntity});
                break;
            }
        }

        protected void DeleteEnity(object sender, EventArgs e)
        {
        }
    }
}
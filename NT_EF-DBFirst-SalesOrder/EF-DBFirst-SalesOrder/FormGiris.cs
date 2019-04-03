using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF_DBFirst_SalesOrder
{
    public partial class FormGiris : Form
    {
        public FormGiris()
        {
            InitializeComponent();
        }
        NorthwindEntitiesConnectionString db = ClassDbSingleTone.GetInstance();
        private Customer secilenCustomer;
        private Customer secilenCustomer1;
        private Order secilenOrder;
        private int secilenCustomerID;

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            Order orders = new Order();
            orders.CustomerID = cmbCustomers.SelectedValue.ToString();
            orders.EmployeeID = (int)cmbEmployees.SelectedValue;
            orders.OrderDate = dtpOrderDate.Value;
            orders.RequiredDate = dtpRequiredDate.Value;
            orders.ShipVia =(int)cmbShipVia.SelectedValue;
            orders.Freight = Convert.ToDecimal(txtFreight.Text);
            orders.ShipCountry = txtCountry.Text;
            orders.ShipAddress = txtAddress.Text;

            db.Orders.Add(orders);

            db.SaveChanges();

            MessageBox.Show("New order has been Inserted to DataBase");

            FormOrderHeaderDetail frm = new FormOrderHeaderDetail(orders.OrderID);
            frm.Show();
            this.Hide();
         }

        private void Doldur()
        {
            cmbCustomers.DisplayMember = "CompanyName";
            cmbCustomers.ValueMember = "CustomerID";
            cmbCustomers.DataSource = db.Customers.ToList();
            
            var elist = db.Employees.Select(x => new
            {
                FullName = x.FirstName + " " + x.LastName,
                x.EmployeeID
            }).ToList();

            cmbEmployees.DataSource = elist;
            cmbEmployees.DisplayMember = "FullName";
            cmbEmployees.ValueMember = "EmployeeID";

            var slist = db.Shippers.Select(x => new
            {
                x.CompanyName,
                x.ShipperID
            }).ToList();

            cmbShipVia.DataSource = slist;
            cmbShipVia.DisplayMember = "CompanyName";
            cmbShipVia.ValueMember = "ShipperID";


            secilenCustomer = db.Customers.Find(cmbCustomers.SelectedValue);

            try
            {

                txtAddress.Text = secilenCustomer.Address.ToString();
            }
            catch (Exception)
            {

                MessageBox.Show("Customer hasn't been Selected.");
            }

            try
            {
                txtCountry.Text = secilenCustomer.Country.ToString();
            }
            catch (Exception)
            {

                MessageBox.Show("Customer hasn't been Selected.");
            }




        }

        private void FormGiris_Load(object sender, EventArgs e)
        {
            Doldur();
        }

        private void cmbCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                secilenCustomer1 = db.Customers.Find(cmbCustomers.SelectedValue);

                txtAddress.Text = secilenCustomer1.Address.ToString();
                txtCountry.Text = secilenCustomer1.Country.ToString();
            }
            catch (Exception)
            {

                MessageBox.Show("Customer hasn't been Selected.");
            }
            
        }
    }
}

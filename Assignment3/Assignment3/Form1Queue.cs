using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//added 3 namespaces
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assignment3
{
    public partial class FormQueue : Form
    {
        //created queue for customer
        Queue<Customer> customerQueue = new Queue<Customer>();

        //created a default text path file
        public static string path = @"queue.txt";
        FileInfo fi = new FileInfo(path);

        Stream stream;
        IFormatter formatter = new BinaryFormatter();

        public FormQueue()
        {
            InitializeComponent();
        }

        private void FormQueue_Load(object sender, EventArgs e)
        {
           

            fi = new FileInfo(path);
            stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);

            //this will make the program deserialize if the file path length is not 0
            if(new FileInfo(path).Length != 0 )
            {
                customerQueue = (Queue<Customer>)formatter.Deserialize(stream);

            }

            stream.Close();
            RefreshAll();
        }

        // creates a new filestream 
        private void FormQueue_FormClosed(object sender, FormClosedEventArgs e)
        {
            stream = new FileStream(path, FileMode.Open, FileAccess.Write);


            formatter.Serialize(stream, customerQueue);

            stream.Close();
        }

        //calls the addcustomer function only because creating arguments inside this function 
        // would cause errors
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddCustomer();
        }

        //removes one unique customer entry 
        private void btnRemove_Click(object sender, EventArgs e)
        {
            Customer removeCustomer = customerQueue.Dequeue();

            RemoveAlert<string>(removeCustomer.Name);

            foreach(Customer customer in customerQueue)
            {
                customer.Position--;
            }

            RefreshAll();
        }

  
        //creates a unique position for each name position and order entry
        private void AddCustomer()
        {
            if (txtName.Text == "" || txtOrder.Text != "")
            {
                customerQueue.Enqueue(new Customer
                {

                    Position = Int32.Parse(lblPosition.Text),
                    Name = txtName.Text,
                    Order = txtOrder.Text
                });
                RefreshAll();
            }

             AddAlert<string>(txtName.Text);
            RefreshAll();   
        }

        //this functions purpose is to clear all of the text from the label and textboxes
        //after each entry
        private void RefreshAll()
        {
            lblPosition.Text = (customerQueue.Count+1).ToString();
            txtName.Text = "";
            txtOrder.Text = "";
            lblOutput.Text = "";

            //displays the entrys according to their queue position
            foreach(Customer customer in customerQueue)
            {
                lblOutput.Text += customer.Position + " - " + customer.Name +
                    " ordered " + customer.Order + "\n";
            }

            if(customerQueue.Count > 0)
            {

                btnRemove.Enabled = true;
            }
            else
            {
                btnRemove.Enabled = false;
            }
        }

        //allows user to go ahead and press enter after submitting name entry
        //without having to use mouse
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtOrder.Text == "")
                {
                    txtOrder.Focus();
                }
                else
                {
                    AddCustomer();
                }
            }
        
        }

        //allows user to go ahead and press enter after submitting name entry
        //without having to use mouse
        private void txtOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtOrder.Text == "")
                {
                    txtOrder.Focus();
                }
                else
                {
                    AddCustomer();
                }
            }
        }

        //informs user with a popup after btnRemove function has been called
        private void RemoveAlert<T>(T data)
        {
            MessageBox.Show($"{data} has been removed from the queue!", "Queue has been changed.",
                MessageBoxButtons.OK,   MessageBoxIcon.Information);
        }
        //informs user with a popup after Add function has been called
        private void AddAlert<T>(T data)
        {
            MessageBox.Show($"{data} has been added to the queue!", "Queue has been changed.",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}

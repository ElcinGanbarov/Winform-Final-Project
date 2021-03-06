﻿using Library_management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_management.Forms
{
    public partial class ShowTheBasketForm : Form
    {
        private OrderDal _orderDal;
        private int _id;
        private Orders orders;
        private BookDal _bookDal;
        private Book _book;
        public event EventHandler Events;
        
        public ShowTheBasketForm(int id)
        {
            _id = id;
            _orderDal = new OrderDal();
            _bookDal = new BookDal();
            InitializeComponent();
        }
        //Order Delete//
        private void Button1_Click(object sender, EventArgs e)
        {
            Orders orders = new Orders
            {
                Id = Convert.ToInt32(dgwShowBasketOrder.CurrentRow.Cells[0].Value)
            };
            _orderDal.Delete(orders);
            LoadAllDataForTheBasket();
        }
        //ShowTheBasketForm_Load Add Order Data//
        private void ShowTheBasketForm_Load(object sender, EventArgs e)
        {
            LoadAllDataForTheBasket();
        }
        //Order View Form Load//
        private void LoadAllDataForTheBasket()
        {
            List<Orders> orders = _orderDal.GetForBasket(_id);
            dgwShowBasketOrder.Rows.Clear();
            foreach (Orders item in orders)
            {
                dgwShowBasketOrder.Rows.Add(item.Id, item.Books.Name, item.BookCount, item.Books.Price, item.Customers.Name, item.Customers.IdentityNumber, item.GivingTime, item.DeadLine, item.Managers.Name);
            }
        }

        //DgwShowBasketOrder_CellClick Choose Order//
        private void DgwShowBasketOrder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id=(int)dgwShowBasketOrder.Rows[e.RowIndex].Cells[0].Value;
             orders = _orderDal.GetById(id);
            _book = _bookDal.GetById(orders.BookId);
            
           
            try
            {
                btnDeleteFromBasketShow.Enabled = true;
                btnUpdateShowBasketForm.Enabled = true;
                textBox1.Text = dgwShowBasketOrder.CurrentRow.Cells[2].Value.ToString();

            }
            catch { }
        }

        //Choose Order Update BookCount//
        private void BtnUpdateShowBasketForm_Click(object sender, EventArgs e)
        {
            int diff = Convert.ToInt32(textBox1.Text) - Convert.ToInt32(orders.BookCount);

            if (_book.Count>= diff)
            {
                int id = Convert.ToInt32(dgwShowBasketOrder.CurrentRow.Cells[0].Value);
                orders = _orderDal.GetById(id);
                orders.BookCount = Convert.ToInt32(textBox1.Text);
                _orderDal.Update(orders);
                LoadAllDataForTheBasket();
                 _book.Count -= diff;
                _bookDal.Update(_book);
                Events?.Invoke(_book, new EventArgs());
            }else if (diff < 0)
            {
                int b = Convert.ToInt32(orders.BookCount) - Convert.ToInt32(textBox1.Text);
                int id = Convert.ToInt32(dgwShowBasketOrder.CurrentRow.Cells[0].Value);
                orders = _orderDal.GetById(id);
                orders.BookCount -=b;
                _orderDal.Update(orders);
                LoadAllDataForTheBasket();
                _book.Count -= diff;
                _bookDal.Update(_book);
                Events?.Invoke(_book, new EventArgs());
            }
            else
            {
                MessageBox.Show("Kitabxanada bu sayda kitab yoxdur");
            }
        }
    }
}

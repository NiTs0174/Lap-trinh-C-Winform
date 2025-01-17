﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab05.BUS;
using Lab05.DAL.Entities;

namespace Lab05.GUI
{
    public partial class frmRegister : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private readonly MajorService majorService = new MajorService();
        public frmRegister()
        {
            InitializeComponent();
        }
        //----------------------------------------------------------------------------------
        private void frmRegister_Load(object sender, EventArgs e)
        {
            try
            {
                var listFacultys = facultyService.GetAll();
                FillFalcultyCombobox(listFacultys);
                var listStudents = studentService.GetAll();
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       
        //----------------------------------------------------------------------------------
        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            this.cmbFaculty.DataSource = listFacultys; 
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }
        //Fill Major Combobox
        private void FillMajorCombobox(List<Major> listMajors)
        {
            //this.cmbMajor.DataSource = listMajors;
            //this.cmbMajor.DisplayMember = "Name";
            //this.cmbMajor.ValueMember = "MajorID";
            // Xóa tất cả các mục hiện có trong combobox.
            cmbMajor.Items.Clear();

            // Thêm từng chuyên ngành vào combobox.
            foreach (Major major in listMajors)
            {
                cmbMajor.Items.Add(major.Name);
            }

            // Chọn mục đầu tiên trong combobox.
            //cmbMajor.SelectedIndex = 0;
        }
        //----------------------------------------------------------------------------------
        private void cmbMajor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Faculty selectedFaculty = cmbFaculty.SelectedItem as Faculty;
            if (selectedFaculty != null)
            {
                var listMajor = majorService.GetAllByFaculty(selectedFaculty.FacultyID); 
                FillMajorCombobox(listMajor);
                var listStudents = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID); 
                BindGrid(listStudents);
            }
        }
        //----------------------------------------------------------------------------------
        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[1].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[2].Value = item.FullName; 
                if (item.Faculty != null)
                {
                    dgvStudent.Rows[index].Cells[3].Value = item.Faculty.FacultyName;
                }
                dgvStudent.Rows[index].Cells[4].Value = item.AverageScore + "";
                if (item.MajorID != null)
                {
                    dgvStudent.Rows[index].Cells[5].Value = item.Major.Name + "";
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
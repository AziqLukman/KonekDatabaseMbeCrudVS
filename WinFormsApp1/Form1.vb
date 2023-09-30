Imports System.Data.Common
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports Microsoft.SqlServer
Imports MySql.Data.MySqlClient
Public Class Form1
    'Public conn As OdbcConnection
    'Public cmd As OdbcCommand
    'Public rd As OdbcDataReader
    Dim conn As MySqlConnection
    Dim dr As MySqlDataReader
    Dim da As MySqlDataAdapter
    Dim cmd As MySqlCommand

    Sub koneksi()
        conn = New MySqlConnection("server=127.0.0.1;userid=root;password='';
        database=db_latihan")
        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
                MsgBox("Koneksi Berhasil", MsgBoxStyle.Exclamation, "informasi")
            End If

        Catch ex As Exception
            MessageBox.Show("Koneksi Gagal" & ex.Message)
        End Try
    End Sub
    Sub bersihkan()
        txtid.Clear()
        txtnpm.Clear()
        txtnama.Clear()
        txtalamat.Clear()
        'jenis_kelamin.Clear()
    End Sub
    Sub tambahdata()
        Try
            Call koneksi()
            Dim input_id As String = txtid.Text
            Dim input_npm As String = txtnpm.Text
            Dim input_nama As String = txtnama.Text
            Dim input_alamat As String = txtalamat.Text
            Dim input_jenis_kelamin As String = txtjenis_kelamin.Text

            Dim query As String = "INSERT INTO tbl_mahasiswa (id, npm, nama, jenis_kelamin, alamat) VALUES (?,?,?,?,?)"

            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("id", input_id)
            cmd.Parameters.AddWithValue("npm", input_npm)
            cmd.Parameters.AddWithValue("nama", input_nama)
            cmd.Parameters.AddWithValue("jenis_kelamin", input_jenis_kelamin)
            cmd.Parameters.AddWithValue("alamat", input_alamat)
            cmd.ExecuteNonQuery()
            MsgBox("Data berhasil disimpan ke database!!")
            tampildata()
            bersihkan()
        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub
    Sub tampildata()
        Call koneksi()
        cmd = New MySqlCommand("Select * from tbl_mahasiswa", conn)
        dr = cmd.ExecuteReader
        DataGridView1.Rows.Clear()

        Do While dr.Read = True
            DataGridView1.Rows.Add(dr(0), dr(1), dr(2), dr(3), dr(4))
        Loop
    End Sub
    Sub hapusdata()
        Call koneksi()
        Dim input_id As String = txtid.Text
        Dim sql As String = "DELETE FROM tbl_mahasiswa WHERE id = ?"
        Try
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("id", input_id)
            cmd.ExecuteNonQuery()
            MsgBox("Data berhasil dihapus")
        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        txtid.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txtnpm.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        txtnama.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        txtalamat.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value
        txtjenis_kelamin.Text = DataGridView1.Rows(e.RowIndex).Cells(4).Value
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Call koneksi()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call tambahdata()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            Dim idToUpdate As Integer = Convert.ToInt32(selectedRow.Cells("id").Value.ToString())

            Dim input_npm As String = txtnpm.Text
            Dim input_nama As String = txtnama.Text
            Dim input_alamat As String = txtalamat.Text
            Dim input_jenis_kelamin As String = txtjenis_kelamin.Text
            Dim sql As String = "UPDATE tbl_mahasiswa SET npm = ?, nama = ?, alamat = ?, jenis_kelamin = ? WHERE id = ?"
            Try
                Call koneksi()
                Dim cmd As New MySqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@npm", input_npm)
                cmd.Parameters.AddWithValue("@nama", input_nama)
                cmd.Parameters.AddWithValue("@alamat", input_alamat)
                cmd.Parameters.AddWithValue("@jenis_kelamin", input_jenis_kelamin)
                cmd.Parameters.AddWithValue("@id", idToUpdate) ' Menggunakan id yang telah dipilih
                cmd.ExecuteNonQuery()
                MessageBox.Show("Data berhasil diupdate.")
                tampildata()
                bersihkan()
            Catch ex As Exception
                MessageBox.Show("Terjadi kesalahan: " & ex.Message)
            Finally
                conn.Close()
            End Try
        Else
            MessageBox.Show("Pilih baris yang ingin diupdate terlebih dahulu.")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call hapusdata()
        Call bersihkan()
    End Sub
End Class

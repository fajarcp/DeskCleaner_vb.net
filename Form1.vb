Public Class Form1
    Dim key, subk As Microsoft.Win32.RegistryKey
    Dim strSystemDir As String = Environment.SystemDirectory.Substring(0, 2)
    Dim AppPath As String = strSystemDir & "\DESKCLEAN\Backtask.exe"             'Needed for startup
    Dim AutoStart As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup) & "/" & IO.Path.GetFileName(AppPath)     'Needed for statrup
    Dim HideFile As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Startup))   'Needed for startup
    Dim LinkPath As String
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        key = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
        key.CreateSubKey("deskclean")
        subk = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\deskclean\", True)
        subk.SetValue("st", "start")
        subk.Close()
        key.Close()


        Try
            IO.File.Copy(AppPath, AutoStart)
            HideFile.IsReadOnly = True                                      'Startup
            HideFile.Attributes = HideFile.Attributes Or IO.FileAttributes.Hidden
        Catch ex As Exception

        End Try
        Button3.Enabled = True
        Button1.Enabled = False
        Button2.Visible = True
        Label1.Visible = True
        Button4.Visible = True
        Label4.Visible = True
        Label4.Text = "Status:Started"
        Label5.Visible = True
        Label5.ForeColor = Color.Green
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If My.Computer.FileSystem.DirectoryExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Deskclean\backup") Then
            If MsgBox("Operation can't be reversed.Do you want to continue?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                My.Computer.FileSystem.DeleteDirectory(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Deskclean\backup", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
        Else
            MsgBox("Backup directory doesn't exists", MsgBoxStyle.OkOnly)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        key = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
        key.CreateSubKey("deskclean")
        subk = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\deskclean\", True)
        subk.SetValue("st", "stop")
        subk.Close()
        key.Close()
        
        Try
            IO.File.Delete(AutoStart)
            HideFile.IsReadOnly = True                                      'Startup
            HideFile.Attributes = HideFile.Attributes Or IO.FileAttributes.Hidden
        Catch ex As Exception

        End Try
        Button1.Enabled = True
        Button3.Enabled = False
        Button2.Visible = False
        Label1.Visible = False
        Button4.Visible = False
        Label4.Visible = True
        Label4.Text = "Status:Not started"
        Label5.Visible = True
        Label5.ForeColor = Color.Red
    End Sub

    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Not IO.File.Exists(strSystemDir & "\DESKCLEAN\DESKCLEANER.exe") Then
            key = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
            subk = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\deskclean\", True)
            subk.SetValue("st", "stop")
            subk.Close()
            key.Close()
            IO.File.Delete(AutoStart)
            HideFile.IsReadOnly = True                                      'Startup
            HideFile.Attributes = HideFile.Attributes Or IO.FileAttributes.Hidden
        End If
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        key = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
        subk = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\deskclean\", True)
        If subk.GetValue("st") = "start" Then
            Button3.Enabled = True
            Button1.Enabled = False
            Button2.Visible = True
            Label1.Visible = True
            Button4.Visible = True
            Label4.Visible = True
            Label4.Text = "Status:Started"
            Label5.Visible = True
            Label5.ForeColor = Color.Green
        Else
            Button1.Enabled = True
            Button3.Enabled = False
            Button2.Visible = False
            Label1.Visible = False
            Button4.Visible = False
            Label4.Visible = True
            Label4.Text = "Status:Not Started"
            Label5.Visible = True
            Label5.ForeColor = Color.Red
        End If
        subk.Close()
        key.Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If My.Computer.FileSystem.DirectoryExists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Deskclean\backup") Then

            Process.Start("explorer.exe", My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Deskclean\backup")
        Else
            MsgBox("Backup directory doesn't exists", MsgBoxStyle.OkOnly)
        End If
    End Sub
End Class

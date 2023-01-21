Imports System.IO
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json.Linq
Imports Bunifu.UI.WinForms

Public Class Form1
    Private Sub BunifuLabel2_Click(sender As Object, e As EventArgs) Handles BunifuLabel2.Click

    End Sub

    Private Sub BunifuHScrollBar1_Scroll(sender As Object, e As Bunifu.UI.WinForms.BunifuHScrollBar.ScrollEventArgs) Handles ScrollVolumen.Scroll
        LabelVolumen.Text = ScrollVolumen.Value.ToString + "%"

    End Sub

    Private Sub BunifuButton1_Click(sender As Object, e As EventArgs) Handles BunifuButton1.Click
        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            ' List files in the folder.
            ListFiles(FolderBrowserDialog1.SelectedPath)
            filesListBox.Enabled = True
        Else
            MsgBox("No selecciono una ruta valida!")
        End If

    End Sub
    Private Sub ListFiles(ByVal folderPath As String)
        filesListBox.Items.Clear()
        LabelRutaTotems.Text = folderPath
        Dim fileNames = My.Computer.FileSystem.GetDirectories(
            folderPath, FileIO.SearchOption.SearchTopLevelOnly)

        For Each fileName As String In fileNames
            Dim thisfolder As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(fileName)
            filesListBox.Items.Add(thisfolder.Name)
        Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListFiles(LabelRutaTotems.Text)
    End Sub

    Private Sub filesListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles filesListBox.SelectedIndexChanged
        Dim FolderTotems As String = LabelRutaTotems.Text
        Dim FolderTotemSelected As String = FolderTotems + "\" + filesListBox.SelectedItem.ToString
        If (Directory.Exists(FolderTotems) And Directory.Exists(FolderTotemSelected)) Then
            If (File.Exists(FolderTotemSelected + "\Control\Rutinas.json")) Then
                Dim dirJsonRutinas = FolderTotemSelected + "\Control\Rutinas.json"
                Dim jsonfile As String = My.Computer.FileSystem.ReadAllText(dirJsonRutinas)
                Dim DataTotemSelected = JObject.Parse(jsonfile)
                informacion_totem(DataTotemSelected, filesListBox.SelectedItem.ToString)

                'activar los botones y opciones de control
                BtnPlayPause.Enabled = True
                BtnPantallaCompleta.Enabled = True
                ScrollVolumen.Enabled = True
            Else
                'desactivar botones y opciones de control
                BtnPlayPause.Enabled = False
                BtnPantallaCompleta.Enabled = False
                ScrollVolumen.Enabled = False
                MsgBox("Error: El archivo Rutinas.json no se encuentra en la carpeta del dispositivo seleccionado, Verifique que selecciono una carpeta perteneciente a un dipositivo Activo")

            End If
        Else
            'desactivar botones y opciones de control
            BtnPlayPause.Enabled = True
            BtnPantallaCompleta.Enabled = True
            ScrollVolumen.Enabled = True
            MsgBox("Lo siento! La Ruta de los totems o la carpeta de control del totem seleccionado o especificado, no existe!")
        End If

    End Sub

    Private Sub informacion_totem(DataTotem As JObject, name As String)
        Dim FolderTotem As String = LabelRutaTotems.Text + "\" + filesListBox.SelectedItem.ToString
        If (File.Exists(FolderTotem + "\Control\captura.jpg")) Then
            ImgCaptureActual.Image = Image.FromFile(FolderTotem + "\Control\captura.jpg")
        End If
        LabelNombreTotem.Text = name
        LabelEstadoActual.Text = DataTotem.SelectToken("estado").ToString
        LabelInfoVolumen.Text = DataTotem.SelectToken("Volumen").ToString
        ScrollVolumen.Value = DataTotem.SelectToken("Volumen").ToString
        LabelModoPantalla.Text = DataTotem.SelectToken("FullScreen").ToString
        'cargar rutina lunes'
        L1.Text = DataTotem.SelectToken("Lunes").SelectToken("Hora_inicio").ToString
        L2.Text = DataTotem.SelectToken("Lunes").SelectToken("Hora_Fin").ToString
        'cargar rutina Martes'
        M1.Text = DataTotem.SelectToken("Martes").SelectToken("Hora_inicio").ToString
        M2.Text = DataTotem.SelectToken("Martes").SelectToken("Hora_Fin").ToString
        'cargar rutina Miercoles'
        MI1.Text = DataTotem.SelectToken("Miercoles").SelectToken("Hora_inicio").ToString
        MI2.Text = DataTotem.SelectToken("Miercoles").SelectToken("Hora_Fin").ToString
        'cargar rutina Jueves'
        J1.Text = DataTotem.SelectToken("Jueves").SelectToken("Hora_inicio").ToString
        J2.Text = DataTotem.SelectToken("Jueves").SelectToken("Hora_Fin").ToString
        'cargar rutina Viernes'
        V1.Text = DataTotem.SelectToken("Viernes").SelectToken("Hora_inicio").ToString
        V2.Text = DataTotem.SelectToken("Viernes").SelectToken("Hora_Fin").ToString
        'cargar rutina Sabado'
        S1.Text = DataTotem.SelectToken("Sabado").SelectToken("Hora_inicio").ToString
        S2.Text = DataTotem.SelectToken("Sabado").SelectToken("Hora_Fin").ToString
        'cargar rutina Domingo'
        D1.Text = DataTotem.SelectToken("Domingo").SelectToken("Hora_inicio").ToString
        D2.Text = DataTotem.SelectToken("Domingo").SelectToken("Hora_Fin").ToString
        'cargar rutina hora de reposo'
        LabelHreposo.Text = "Descansar  Desde: " + DataTotem.SelectToken("HoraDeReposo").SelectToken("desde").ToString + " Hasta: " + DataTotem.SelectToken("HoraDeReposo").SelectToken("hasta").ToString
        'mostrar ventana info totem'
        Me.PagesGlobal.SelectedTab = TabPage2
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Me.PagesGlobal.SelectedTab = TabPage1
    End Sub

    Private Sub ImgCaptureActual_Click(sender As Object, e As EventArgs)
        Dim FolderTotem As String = LabelRutaTotems.Text + "\" + filesListBox.SelectedItem.ToString
        If (File.Exists(FolderTotem + "\Control\captura.jpg")) Then
            Form2.Show()
            Dim sizeImg = Image.FromFile(FolderTotem + "\Control\captura.jpg").Size
            Form2.Size = New Drawing.Size(sizeImg)
            Form2.BackgroundImage = Image.FromFile(FolderTotem + "\Control\captura.jpg")
            Form2.StartPosition = FormStartPosition.CenterScreen
        End If


    End Sub

    Private Sub BunifuCheckBox1_CheckedChanged(sender As Object, e As Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs) Handles CheckMiniatura.CheckedChanged
        If CheckMiniatura.CheckState Then
            TimerCaptureP.Start()
        Else
            TimerCaptureP.Stop()
        End If
    End Sub

    Private Sub TimerCaptureP_Tick(sender As Object, e As EventArgs) Handles TimerCaptureP.Tick
        If (CheckMiniatura.Checked) Then
            Dim FolderTotem As String = LabelRutaTotems.Text + "\" + filesListBox.SelectedItem.ToString
            If (File.Exists(FolderTotem + "\Control\captura.jpg")) Then
                ImgCaptureActual.Image = Image.FromFile(FolderTotem + "\Control\captura.jpg")
            End If
            If (Form2.WindowState = FormWindowState.Normal) Then
                Dim sizeImg = Image.FromFile(FolderTotem + "\Control\captura.jpg").Size
                Form2.Size = New Drawing.Size(sizeImg)
                Form2.BackgroundImage = Image.FromFile(FolderTotem + "\Control\captura.jpg")
                Form2.StartPosition = FormStartPosition.CenterScreen
            End If
        End If

    End Sub

    Private Sub ScrollVolumen_ValueChanged(sender As Object, e As BunifuHScrollBar.ValueChangedEventArgs) Handles ScrollVolumen.ValueChanged
        LabelVolumen.Text = ScrollVolumen.Value.ToString + "%"
    End Sub

    Private Sub BtnPantallaCompleta_Click(sender As Object, e As EventArgs) Handles BtnPantallaCompleta.Click

    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Dim Response = MsgBox("Seguro deseas salir del programa?", vbYesNo Or vbCritical Or vbDefaultButton2, "Salir")
        If Response = vbYes Then    ' User chose Yes.
            Me.Close()
        Else    ' User chose No.

        End If
    End Sub
End Class

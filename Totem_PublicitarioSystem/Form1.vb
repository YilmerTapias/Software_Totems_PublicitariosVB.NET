Imports System.IO
Imports WMPLib
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json.Linq
Imports System.Drawing.Imaging
Imports System.Diagnostics

Public Class Form1
    'create playlist
    Dim newPlayList As WMPLib.IWMPPlaylist
    Dim DirMedia As String = My.Computer.FileSystem.CurrentDirectory.ToString
    Dim JsonData As JObject
    Dim JsonRutinas As JObject
    Dim nombreDispositivo As String
    Dim NombreUserPc As String = System.Environment.UserName
    Dim Dia_Semana = DateTime.Now.DayOfWeek
    'variables para la rutina del dia
    Dim Hora_inicio As String
    Dim Hora_fin As String
    'almacena el numero de videos en el directorio para saber cuando el usuario agregue uno nuevo
    Dim NumeroDeVideos As Integer



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Inicio()

    End Sub
    Private Sub Inicio()
        Dim DirJsonConfig As String = My.Computer.FileSystem.CurrentDirectory.ToString + "\Config.json"
        If (File.Exists(DirJsonConfig)) Then
            'Json en red
            'Dim json As String = New System.Net.WebClient().DownloadString("http://time.jsontest.com/")
            Dim jsonfile As String = My.Computer.FileSystem.ReadAllText(DirJsonConfig)
            JsonData = JObject.Parse(jsonfile)
            nombreDispositivo = JsonData.SelectToken("Nombre").ToString
            'JsonData = parsejson.SelectToken("Nombre") accesar dato del archivo Json
            'Console.WriteLine("JsonData: " + JsonData.SelectToken("Nombre").ToString)
        Else
            MsgBox("No se ha encontrado ningún archivo de configuración (.json) en la carpeta raíz del programa")
            My.Forms.Form1.Close()
        End If

        EliminarPlaylistAntiguos()
        'cargamos los videos a reproducir
        MediaPlayer.CreateControl()
        newPlayList = MediaPlayer.playlistCollection.newPlaylist(“VideosPresentacion”)
        Dim ModoControl As String = JsonData.SelectToken("ModoControl").ToString

        If (ModoControl = "local") Then
            Dim RutaVideosLocal As String = "C:\Users\" + NombreUserPc + "\Videos\" + JsonData.SelectToken("Nombre").ToString

            If (Directory.Exists(RutaVideosLocal)) Then
            Else
                MsgBox("Se esta creando una carpeta local para el almacenamiento de los videos a reproducir. en la ruta: " + RutaVideosLocal)
                Directory.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.MyMusic.ToString + "\Listas de reproducción")
                Directory.CreateDirectory(RutaVideosLocal)
                Directory.CreateDirectory(RutaVideosLocal + "\control")
                MsgBox("Creando Carpeta de Control")
                File.Copy(My.Computer.FileSystem.CurrentDirectory.ToString + "\Rutinas.json", RutaVideosLocal + "\control\Rutinas.json")
            End If
            Dim folderVideos As New DirectoryInfo(RutaVideosLocal)
            NumeroDeVideos = folderVideos.GetFiles().Count
            If (folderVideos.GetFiles().Count <= 0) Then
                MsgBox("No se encontraron videos en el directorio local. Agregue los videos para empezar la reproduccion automatica")
            Else
                For Each foundFile As FileInfo In folderVideos.GetFiles()
                    newPlayList.appendItem(MediaPlayer.newMedia(RutaVideosLocal + "\" + foundFile.Name))
                Next
            End If
            'cargamos la rutina del día
            cargar_rutina(ModoControl, RutaVideosLocal)
        ElseIf (ModoControl = "usb") Then
            'buscar archivo de configuración y videos a reproducir en una USB con la letra de unidad (S:)
        ElseIf (ModoControl = "googledrive") Then
            'codigo
        End If

        'play the list
        MediaPlayer.Visible = True
        MediaPlayer.currentPlaylist = newPlayList
        MediaPlayer.settings.setMode("loop", True)
        MediaPlayer.stretchToFit = False
        MediaPlayer.uiMode = "none"
        MediaPlayer.Ctlcontrols.play()
    End Sub

    Private Sub EliminarPlaylistAntiguos()
        'eliminamos los archivos que se generan automaticamente al crear un newplaylist en el directorio MyMusic del computador
        Dim dirPlayListAntiguos As New DirectoryInfo(My.Computer.FileSystem.SpecialDirectories.MyMusic.ToString + "\Listas de reproducción")
        If (Directory.Exists(dirPlayListAntiguos.FullName)) Then
            If (My.Computer.FileSystem.GetFiles(dirPlayListAntiguos.FullName).Count > 0) Then
                For Each foundFile As FileInfo In dirPlayListAntiguos.GetFiles()
                    File.Delete(foundFile.FullName)
                Next
            End If
        Else
            Directory.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.MyMusic.ToString + "\Listas de reproducción")
        End If


    End Sub

    Private Sub cargar_rutina(modocontrol As String, rutavideos As String)

        Dim DirRutinasJson As String = My.Computer.FileSystem.CurrentDirectory.ToString + "\Rutinas.json"
        'MsgBox("C:\Users\" + NombreUserPc + "\Videos\" + JsonData.SelectToken("Nombre").ToString + "\control\Rutinas.json")
        If (File.Exists("C:\Users\" + NombreUserPc + "\Videos\" + JsonData.SelectToken("Nombre").ToString + "\control\Rutinas.json")) Then
            DirRutinasJson = "C:\Users\" + NombreUserPc + "\Videos\" + JsonData.SelectToken("Nombre").ToString + "\control\Rutinas.json"

        End If
        Dim jsonfile As String = My.Computer.FileSystem.ReadAllText(DirRutinasJson)
        JsonRutinas = JObject.Parse(jsonfile)

        Select Case Dia_Semana

            Case 0
                'domingo
                Hora_inicio = JsonRutinas.SelectToken("Domingo").SelectToken("Hora_inicio").ToString()
                Hora_fin = JsonRutinas.SelectToken("Domingo").SelectToken("Hora_Fin").ToString()

            Case 1
                'lunes
                Hora_inicio = JsonRutinas.SelectToken("Lunes").SelectToken("Hora_inicio").ToString()
                Hora_fin = JsonRutinas.SelectToken("Lunes").SelectToken("Hora_Fin").ToString()
            Case 2
                'martes
                Hora_inicio = JsonRutinas.SelectToken("Martes").SelectToken("Hora_inicio").ToString()
                Hora_fin = JsonRutinas.SelectToken("Martes").SelectToken("Hora_Fin").ToString()
            Case 3
                'miercoles
                Hora_inicio = JsonRutinas.SelectToken("Miercoles").SelectToken("Hora_inicio").ToString()
                Hora_fin = JsonRutinas.SelectToken("Miercoles").SelectToken("Hora_Fin").ToString()
            Case 4
                'jueves
                Hora_inicio = JsonRutinas.SelectToken("Jueves").SelectToken("Hora_inicio").ToString()
                Hora_fin = JsonRutinas.SelectToken("Jueves").SelectToken("Hora_Fin").ToString()
            Case 5
                'viernes
                Hora_inicio = JsonRutinas.SelectToken("Viernes").SelectToken("Hora_inicio").ToString()
                Hora_fin = JsonRutinas.SelectToken("Viernes").SelectToken("Hora_Fin").ToString()
            Case 6
                'sabado
                Hora_inicio = JsonRutinas.SelectToken("Sabado").SelectToken("Hora_inicio").ToString()
                Hora_fin = JsonRutinas.SelectToken("Sabado").SelectToken("Hora_Fin").ToString()
            Case Else
                Hora_inicio = JsonRutinas.SelectToken("Lunes").SelectToken("Hora_inicio").ToString()
                Hora_fin = JsonRutinas.SelectToken("Lunes").SelectToken("Hora_Fin").ToString()

        End Select
        Timer1Hora.Start()

    End Sub

    Private Sub AxWindowsMediaPlayer1_StatusChange(sender As Object, e As EventArgs) Handles MediaPlayer.StatusChange

        If (MediaPlayer.playState = WMPLib.WMPPlayState.wmppsPlaying) Then
            ' MediaPlayer.fullScreen = False
        End If



    End Sub

    Private Sub Timer1Hora_Tick(sender As Object, e As EventArgs) Handles Timer1Hora.Tick
        Dim DirRutinasJson As String = "C:\Users\" + NombreUserPc + "\Videos\" + JsonData.SelectToken("Nombre").ToString + "\control\Rutinas.json"
        Dim jsonfile As String = My.Computer.FileSystem.ReadAllText(DirRutinasJson)
        JsonRutinas = JObject.Parse(jsonfile)

        If (Hora_inicio <= DateTime.Now.ToString("HH:mm") And Hora_fin >= DateTime.Now.ToString("HH:mm")) Then
            Dim RutaVideosLocal As String = "C:\Users\" + NombreUserPc + "\Videos\" + nombreDispositivo
            Dim folderVideos As New DirectoryInfo(RutaVideosLocal)
            Dim NumeroDeVideos2 As Integer = folderVideos.GetFiles().Count

            'cambiar estado en tiempo real play, pause o stop
            If (JsonRutinas.SelectToken("estado").ToString = "pause") Then
                MediaPlayer.Ctlcontrols.pause()
            ElseIf (JsonRutinas.SelectToken("estado").ToString = "stop") Then
                MediaPlayer.Ctlcontrols.stop()
            ElseIf (JsonRutinas.SelectToken("estado").ToString = "play") Then
                MediaPlayer.Ctlcontrols.play()
            Else
                MediaPlayer.Ctlcontrols.play()
            End If

            'cambiar volumen en tiempo real
            Dim volumen As Integer = JsonRutinas.SelectToken("Volumen").ToString
            If (volumen < 0 Or volumen > 100) Then
                MediaPlayer.settings.volume = 50
            Else
                MediaPlayer.settings.volume = Integer.Parse(volumen)
            End If

            'tomar captura de pantalla
            If (Boolean.Parse(JsonRutinas.SelectToken("CapturarPantalla").ToString) = True) Then
                'inicia el timer para que haga una captura de pantalla cada 5segundos'
                TimerCapturaPantalla.Start()
            Else
                TimerCapturaPantalla.Stop()
            End If


            'cambiar pantalla completa en tiempo real
            Dim fullscreen As Boolean = Boolean.Parse(JsonRutinas.SelectToken("FullScreen").ToString)
            If (fullscreen = True And MediaPlayer.playState = WMPLib.WMPPlayState.wmppsPlaying) Then
                MediaPlayer.fullScreen = True
            Else
                MediaPlayer.fullScreen = False
            End If

            If (NumeroDeVideos <> NumeroDeVideos2) Then
                MediaPlayer.Ctlcontrols.stop()
                Inicio()
            End If
            If (NumeroDeVideos <> NumeroDeVideos2) Then
                MediaPlayer.Ctlcontrols.stop()
                Inicio()
            End If
        Else
            MediaPlayer.Ctlcontrols.stop()
            Apagar_Dispositivo()
        End If

    End Sub
    Private Sub Apagar_Dispositivo()
        Dim Apagar As New Process
        Apagar.StartInfo.CreateNoWindow = True     'Sin ventana
        Apagar.StartInfo.UseShellExecute = False    'Sin Shell
        Apagar.StartInfo.FileName = "Shutdown"      'Llamar a Shutdown
        'Apagar.StartInfo.Arguments = "/s /t5"       'En cinco segundos
        ' Apagar.StartInfo.Arguments = "?"
        Apagar.Start("shutdown.exe", " -s -t 5 -f")
        Me.Close()
    End Sub
    Private Sub Form1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Control And e.Alt And e.KeyCode = Keys.S Then
            MediaPlayer.fullScreen = False
            MediaPlayer.Ctlcontrols.stop()
            My.Forms.Form1.Close()
        End If
    End Sub


    Private Sub o_Tick(sender As Object, e As EventArgs) Handles TimerCapturaPantalla.Tick
        If (MediaPlayer.playState = WMPLib.WMPPlayState.wmppsPlaying) Then
            ' SendKeys.SendWait("+{PRTSC}")
            'capturar, guardar y mostrar la captura en el PictureBox'
            ' Dim ventana As New Bitmap(400, 600)
            ' ventana = CType(Clipboard.GetDataObject().GetData("Bitmap"), Bitmap)
            ' Dim nombrejpg As String = "captura.jpg"
            ' ventana.Save("C:\Users\" + NombreUserPc + "\Videos\" + JsonData.SelectToken("Nombre").ToString + "\control\" + nombrejpg, ImageFormat.Jpeg)

            'ImgPantalla.Image = ventana
        End If

    End Sub
End Class

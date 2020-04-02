Public Class Form1
    Friend SteamLib As String = ""
    Friend SteamEnable As Boolean = False
    Friend PathSteam As String = ""
    Friend SteamRimEnable As Boolean = False
    Friend RimEnable As Boolean = False
    Friend PathSteamRim As String = ""
    Friend PathRim As String = ""
    Friend SteamRimWorkshopEnable As Boolean = False
    Friend PathSteamRimWorkshop As String = ""
    Friend PathLang As String = ""
    Friend PathMod As String = ""
    Friend RimSteamMod As String()
    Friend RimLocMod As String()
    Friend RimSteamModFullPath As String()
    Friend RimLocModFullPath As String()
    Friend RimSteamModName As String()
    Friend RimLocModName As String()
    Friend ModNames() As (String, String, String)   '_name, _path, _id
    Friend CountSteamMods As Integer = 0
    Friend CountWorkMods As Integer = 0
    Friend PathDataDir As String = "f:\GitHub\Rim Utility\Data\"
    Friend XTest() As (Id As String, Name As String, Path As String)


    Public Function CheckDataDir(_modID As String) As Boolean
        If IO.Directory.Exists(PathDataDir + _modID) Then
            TextBox4.Text = "Данный мод присутствует в базе"
            Return (True)
        Else
            TextBox4.Text = "Данный мод отсутствует в базе"
            ListBox2.Items.Clear()
            Return (False)
        End If

    End Function

    Public Function GetDataLang(_modID) As Boolean
        Dim s As String = PathDataDir + _modID + "\Languages"
        If IO.Directory.Exists(s) Then
            Dim lll As Integer = s.Length
            Dim ddd = IO.Directory.GetDirectories(s)
            Dim xxx = ddd.Count
            For i = 0 To xxx - 1
                If ddd.ElementAt(i).Length > lll Then
                    ddd(i) = ddd.ElementAt(i).Substring(lll + 1)
                End If
            Next

            ListBox2.Items.Clear()
            ListBox2.Items.AddRange(ddd)
            If xxx > 0 Then ListBox2.SelectedIndex = 0
        End If
        If ListBox2.Items.Count > 0 Then Return (True) Else Return (False)

    End Function

    Public Function getmods() As Boolean
        TextBox1.Text = ModNames(ListBox1.SelectedIndex).Item1
        TextBox2.Text = ModNames(ListBox1.SelectedIndex).Item2
        TextBox3.Text = ModNames(ListBox1.SelectedIndex).Item3
        If CheckDataDir(ModNames(ListBox1.SelectedIndex).Item3) Then
            GetDataLang(ModNames(ListBox1.SelectedIndex).Item3)
        End If
        Return (True)
    End Function
    Public Function SortMods() As Boolean
        Dim z = ModNames.OrderBy(Function(s) s.Item1)
        ModNames = z.ToArray
        Return (True)
    End Function
    Public Function RimLang() As Boolean

        If SteamRimEnable Then PathLang = PathSteamRim + "\Data\Core\Languages\"
        If RimEnable Then PathLang = PathRim + "\Data\Core\Languages\"
        Dim lll As Integer = PathLang.Length
        Dim ddd = IO.Directory.GetFiles(PathLang)
        Dim xxx = ddd.Count
        If xxx > 0 Then
            For i = 0 To xxx - 1
                If ddd.ElementAt(i).Length > lll Then
                    ddd(i) = ddd.ElementAt(i).Substring(lll)
                    If ddd.ElementAt(i).ToLower <> "english" Then ddd(i) = Mid(ddd.ElementAt(i), 1, Len(ddd.ElementAt(i)) - 4).ToString
                End If
            Next
        End If
        If SteamRimEnable Or RimEnable Then
            ComboBox1.Items.Clear()
            ComboBox1.Items.Add("English")
            ComboBox1.Items.AddRange(ddd)
            ComboBox1.SelectedIndex = 0
            Return (True)
        Else
            ComboBox1.Items.Clear()
            ComboBox1.Items.Add("English")
            ComboBox1.SelectedIndex = 0
            Return (False)
        End If

        Return (0)
    End Function

    Public Function NameMod(FullPathMod As String) As String
        Dim ModName As String = "unknow"
        If IO.Directory.Exists(FullPathMod + "\About") Then
            If IO.File.Exists(FullPathMod + "\About\About.xml") Then
                Dim doc As XDocument = XDocument.Load(FullPathMod + "\About\About.xml")
                ModName = doc.Root.Element("name").Value.ToString
            End If
        End If

        Return (ModName)
    End Function

    Public Function GetID(_path As String) As String
        Dim PFD As String = vbEmpty
        If IO.File.Exists(_path + "\About\PublishedFileId.txt") Then PFD = IO.File.ReadAllText(_path + "\About\PublishedFileId.txt")
        Return (PFD)
    End Function

    Public Function FillNameMod() As Boolean

        If SteamRimEnable Then
            Dim ListMods = My.Computer.FileSystem.GetDirectories(PathSteamRim + "\Mods")
            Dim WListMods = My.Computer.FileSystem.GetDirectories(PathSteamRimWorkshop)
            Dim SteamID As String = vbEmpty
            CountSteamMods = ListMods.Count
            CountWorkMods = WListMods.Count
            ReDim ModNames(CountSteamMods + CountWorkMods - 1)
            ListBox1.Items.Clear()
            If CountSteamMods > 0 Then
                For i = 0 To CountSteamMods - 1
                    ModNames(i).Item1 = NameMod(ListMods(i))
                    ModNames(i).Item2 = ListMods(i)
                    ModNames(i).Item3 = GetID(ModNames(i).Item2)
                Next
            End If
            If CountWorkMods > 0 Then
                For i = 0 To CountWorkMods - 1
                    ModNames(i + CountSteamMods).Item1 = NameMod(WListMods(i))
                    ModNames(i + CountSteamMods).Item2 = WListMods(i)
                    ModNames(i + CountSteamMods).Item3 = GetID(ModNames(i + CountSteamMods).Item2)
                Next
            End If
        End If
        If CountSteamMods + CountWorkMods > 0 Then
            SortMods()
            For i = 0 To CountSteamMods + CountWorkMods - 1
                ListBox1.Items.Add(ModNames(i).Item1.ToString)
            Next
        End If

        Return (True)
    End Function

    Public Function FillModList() As Boolean
        Dim lll As Integer = 0
        Dim CurModPath As String = ""

        If SteamRimEnable Then CurModPath = PathSteamRim + "\Mods"
        If RimEnable Then CurModPath = PathRim + "\Mods"
        lll = CurModPath.Length
        Dim aaa = IO.Directory.GetDirectories(CurModPath)
        Dim xxx = aaa.Count
        Dim zzz As String = ""

        If SteamRimEnable Then
            RimSteamModFullPath = IO.Directory.GetDirectories(CurModPath)
            RimSteamMod = aaa
        End If

        If RimEnable Then
            RimLocModFullPath = IO.Directory.GetDirectories(CurModPath)
            RimLocMod = aaa
        End If

        For i = 0 To xxx - 1
            If SteamRimEnable Then
                RimSteamMod(i) = aaa(i).Substring(lll + 1)
            End If

            If RimEnable Then
                RimLocMod(i) = aaa(i).Substring(lll + 1)
            End If

        Next

        If SteamRimEnable Then
            ListBox1.Items.Clear()
            FillNameMod()
        Else
            ListBox1.Items.Clear()
        End If

        ListBox1.Refresh()

        Return (True)
    End Function

    Public Function CheckRegSteam() As Boolean
        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", Nothing) Is Nothing Then
            ToolStripStatusLabel1.Text = "Steam: не найден."
            Return (False)
        Else
            PathSteam = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", Nothing)
            ToolStripStatusLabel1.Text = "Steam: установлен."
            Return (True)
        End If

    End Function

    Public Function CheckRimSteam() As Boolean
        Dim aaa As String = ""
        Dim bbb As Integer = 0

        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam\Apps\294100", "Installed", Nothing) Is Nothing Then
            ToolStripStatusLabel2.Text = "Rimword: не найден."
            Return (False)
        Else
            If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Valve\Steam\Apps\294100", "Installed", Nothing) = 1 Then
                ToolStripStatusLabel2.Text = "Rimword: установлен."
                Dim zzz = My.Computer.FileSystem.ReadAllText(PathSteam + "\config\config.vdf")
                bbb = InStr(zzz, "BaseInstallFolder_1")
                If bbb > 0 Then
                    aaa = Trim(zzz.Substring(bbb + 22))
                    bbb = InStr(aaa, """")
                    aaa = Mid(aaa, 1, bbb - 1)
                    While InStr(aaa, "\\")
                        bbb = InStr(aaa, "\\")
                        aaa = aaa.Remove(bbb, 1)
                    End While
                    PathSteamRim = aaa + "\SteamApps\common\RimWorld"
                    PathSteamRimWorkshop = aaa + "\SteamApps\workshop\content\294100"

                    If IO.File.Exists(PathSteamRim + "\Version.txt") Then
                        Dim vvv As String = My.Computer.FileSystem.ReadAllText(PathSteamRim + "\Version.txt")
                        vvv = vvv.Trim(vbCr, vbLf)
                        ToolStripStatusLabel3.Text = "Версия игры: " + vvv
                    Else
                        ToolStripStatusLabel3.Text = "Версия игры: неизвестна"
                    End If
                End If
            End If
            Return (True)
        End If

    End Function


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SteamEnable = CheckRegSteam()
        SteamRimEnable = CheckRimSteam()
        RimLang()
        FillModList()
        ListBox1.SelectedIndex = 0
        getmods()



    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        getmods()
    End Sub

End Class
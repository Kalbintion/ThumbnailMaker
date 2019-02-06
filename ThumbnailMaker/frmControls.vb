Public Class frmControls
    Private ver As String = Application.ProductVersion

    Private Sub tmrPrevUp_Tick(sender As System.Object, e As System.EventArgs) Handles tmrPrevUp.Tick
        updatePreview()
    End Sub

    Private Sub updatePreview()
        Try
            Static Dim bmp As Bitmap
            Static Dim rect As Rectangle

            If bmp IsNot Nothing Then
                bmp.Dispose()
                rect = Nothing
            End If

            Using g As Graphics = frmBmp.CreateGraphics()
                bmp = New Bitmap(frmBmp.Width, frmBmp.Height, g)
                rect = New Rectangle(0, 0, frmBmp.Width, frmBmp.Height)
                frmBmp.DrawToBitmap(bmp, rect)
                pbPrev.Image = bmp
            End Using
        Catch ex As ArgumentException
            frmBmp.Dispose()
            frmBmp.Show()
            frmBmp.Hide()
        End Try
    End Sub

    Private Sub frmControls_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Toggle image form showing to hiding so its loaded into memory properly
        frmBmp.Show()
        frmBmp.Hide()

        ' Get font list
        Dim fontFamilies() As FontFamily = New Drawing.Text.InstalledFontCollection().Families
        For Each fontFamily As FontFamily In fontFamilies
            cbFontName.Items.Add(fontFamily.Name)
        Next

        ' Get supported res list
        Dim srd As Dictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)
        Dim srl As ArrayList = ScreenRes.GetSupportedResolutions()
        For Each sr As ScreenRes.SCREENDATA In srl
            If Not srd.TryGetValue(sr.width & "x" & sr.height, Nothing) Then
                srd.Add(sr.width & "x" & sr.height, True)
                cbQuickSet.Items.Add(sr.width & "x" & sr.height)
            End If

        Next

        ' Determine file association exists
        FileAssocCheck()

        ' Determine if file path was given via command line
        Dim clArgs() As String = Environment.GetCommandLineArgs
        If clArgs.Length > 1 Then
            Dim possiblePath As String = clArgs(1) ' We expect it to be at first position
            If FileIO.FileSystem.FileExists(possiblePath) Then ' we have a proper file
                LoadFileData(possiblePath)
            End If
        End If

    End Sub

    Private Sub tcCore_Selected(sender As Object, e As System.Windows.Forms.TabControlEventArgs) Handles tcCore.Selected
        Dim selected As TabControl = sender

        Select Case selected.SelectedTab.Text
            Case "Image"
                lblStatus.Text = "Use this tab to incorporate basic image information."
            Case "Font"
                lblStatus.Text = "Use this tab to modify the episode numbers appearance."
            Case "Controls"
                lblStatus.Text = "Use this tab to start the rendering process."
            Case "Preview"
                lblStatus.Text = "Use this tab to preview the image before rendering."
        End Select
    End Sub


    Private Sub updateEpFont()
        Dim fontStyle As System.Drawing.FontStyle = fontStyle.Regular
        If chkFontBold.Checked Then
            fontStyle = fontStyle Or System.Drawing.FontStyle.Bold
        End If
        If chkFontItalic.Checked Then
            fontStyle = fontStyle Or System.Drawing.FontStyle.Italic
        End If
        If chkFontStrike.Checked Then
            fontStyle = fontStyle Or System.Drawing.FontStyle.Strikeout
        End If
        If chkFontUnderline.Checked Then
            fontStyle = fontStyle Or System.Drawing.FontStyle.Underline
        End If

        Dim nF As System.Drawing.Font = New System.Drawing.Font(cbFontName.Text, nudFontSize.Value, fontStyle)
        frmBmp.lblEp.Font = nF

        updatePreview()
    End Sub

    Private Sub cbFontName_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbFontName.SelectedIndexChanged
        updateEpFont()
    End Sub

    Private Sub nudFontSize_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudFontSize.ValueChanged
        updateEpFont()
    End Sub

    Private Sub chkFontBold_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkFontBold.CheckedChanged
        updateEpFont()
    End Sub

    Private Sub chkFontItalic_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkFontItalic.CheckedChanged
        updateEpFont()
    End Sub

    Private Sub chkFontUnderline_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkFontUnderline.CheckedChanged
        updateEpFont()
    End Sub

    Private Sub chkFontStrike_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkFontStrike.CheckedChanged
        updateEpFont()
    End Sub

    Private Sub cbFontLocation_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbFontLocation.SelectedIndexChanged
        cbFontLocation.BackColor = Color.White
        Select Case cbFontLocation.Text
            Case "Top Left"
                frmBmp.lblEp.TextAlign = ContentAlignment.TopLeft
            Case "Top Middle"
                frmBmp.lblEp.TextAlign = ContentAlignment.TopCenter
            Case "Top Right"
                frmBmp.lblEp.TextAlign = ContentAlignment.TopRight
            Case "Middle Left"
                frmBmp.lblEp.TextAlign = ContentAlignment.MiddleLeft
            Case "Middle Middle"
                frmBmp.lblEp.TextAlign = ContentAlignment.MiddleCenter
            Case "Middle Right"
                frmBmp.lblEp.TextAlign = ContentAlignment.MiddleRight
            Case "Bottom Left"
                frmBmp.lblEp.TextAlign = ContentAlignment.BottomLeft
            Case "Bottom Middle"
                frmBmp.lblEp.TextAlign = ContentAlignment.BottomCenter
            Case "Bottom Right"
                frmBmp.lblEp.TextAlign = ContentAlignment.BottomRight
            Case Else
                cbFontLocation.BackColor = Color.Red
        End Select
    End Sub

    Private Sub btnFile_Click(sender As System.Object, e As System.EventArgs) Handles btnFile.Click
        ofd.Title = "Open image file..."
        ofd.Filter = "Image Files|*.gif;*.png;*.bmp;*.jpg;*.jpeg;*.wmf|JPG Files|*.jpeg;*.jpg|Bitmap Files|*.bmp|PNG Files|*.png|Windows Metafile|*.wmf|All Files|*.*"
        Dim ret As DialogResult = ofd.ShowDialog(Me)
        If Not ret = DialogResult.Cancel Then
            ' We're loading a new image
            frmBmp.pbImg.ImageLocation = ofd.FileName
            txtFile.Text = ofd.FileName
        End If
    End Sub

    Private Sub txtFile_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtFile.TextChanged
        If System.IO.File.Exists(txtFile.Text) Then
            txtFile.BackColor = Color.White
            ofd.FileName = txtFile.Text
            frmBmp.pbImg.ImageLocation = txtFile.Text
        Else
            txtFile.BackColor = Color.Red
        End If
    End Sub

    Private Sub btnFontColor_Click(sender As System.Object, e As System.EventArgs) Handles btnFontColor.Click
        Dim ret As DialogResult = cd.ShowDialog(Me)
        If Not ret = DialogResult.Cancel Then
            ' We're setting a color
            frmBmp.lblEp.ForeColor = cd.Color
            txtColorHex.Text = String.Format("{0:X2}{1:X2}{2:X2}", cd.Color.R, cd.Color.G, cd.Color.B)
        End If
    End Sub

    Private Sub txtColorHex_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtColorHex.TextChanged
        txtColorHex.BackColor = Color.White
        If txtColorHex.Text.Length <> 6 And txtColorHex.Text.Length <> 3 Then
            txtColorHex.BackColor = Color.Red
            Return
        End If

        Dim toParseHex As String = txtColorHex.Text

        If txtColorHex.Text.Length = 3 Then
            ' We have a 3 letter hex code
            toParseHex = StrDup(2, txtColorHex.Text.Substring(0, 1))
            toParseHex &= StrDup(2, txtColorHex.Text.Substring(1, 1))
            toParseHex &= StrDup(2, txtColorHex.Text.Substring(2, 1))
        End If

        Dim nR As Integer = Convert.ToInt32(toParseHex.Substring(0, 2), 16)
        Dim nG As Integer = Convert.ToInt32(toParseHex.Substring(2, 2), 16)
        Dim nB As Integer = Convert.ToInt32(toParseHex.Substring(4, 2), 16)

        cd.Color = Color.FromArgb(nR, nG, nB)
        frmBmp.lblEp.ForeColor = Color.FromArgb(nR, nG, nB)
    End Sub

    Private Sub nudImageWidth_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudImageWidth.ValueChanged
        frmBmp.Width = nudImageWidth.Value
    End Sub

    Private Sub nudImageHeight_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudImageHeight.ValueChanged
        frmBmp.Height = nudImageHeight.Value
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        sfd.Title = "Save Thumbnail Settings..."
        sfd.Filter = "Thumbnail Maker Files|*.ktm|All Files|*.*"
        Dim ret As DialogResult = sfd.ShowDialog()
        If Not ret = DialogResult.Cancel Then
            ' We're saving!
            Dim out As String = ""
            out &= "VERSION=" & Me.ver & Environment.NewLine

            ' Image Tab Data
            out &= "Image_Path=" & txtFile.Text & Environment.NewLine
            out &= "Image_Size=" & nudImageWidth.Value & "x" & nudImageHeight.Value & Environment.NewLine
            out &= "Image_Start=" & nudEpStart.Value & Environment.NewLine
            out &= "Image_Stop=" & nudEpStart.Value & Environment.NewLine
            out &= "Image_Prev=" & nudEpPrev.Value & Environment.NewLine

            ' Font Tab Data
            out &= "Font_Family=" & cbFontName.Text & Environment.NewLine
            out &= "Font_Size=" & nudFontSize.Value & Environment.NewLine
            out &= "Font_Style=" & chkFontBold.Checked & ", " & chkFontItalic.Checked & ", " & chkFontUnderline.Checked & ", " & chkFontStrike.Checked & Environment.NewLine
            out &= "Font_Loc=" & cbFontLocation.Text & Environment.NewLine
            out &= "Font_Color=" & txtColorHex.Text & Environment.NewLine

            ' Control Tab Data
            out &= "Save_Loc=" & txtSaveLoc.Text & Environment.NewLine
            out &= "Save_File=" & txtSaveFile.Text & Environment.NewLine
            out &= "Save_Format=" & cbFormat.Text & Environment.NewLine

            System.IO.File.WriteAllText(sfd.FileName, out)
        End If
    End Sub

    Private Sub LoadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        ofd.Title = "Load Thumbnail Setings..."
        ofd.Filter = "Thumbnail Maker Files|*.ktm|All Files|*.*"
        Dim ret As DialogResult = ofd.ShowDialog()
        If Not ret = DialogResult.Cancel Then
            ' We're loading!
            LoadFileData(ofd.FileName)
        End If
    End Sub

    Private Sub LoadFileData(source As String)
        Dim data As String = System.IO.File.ReadAllText(source)
        Dim lines() As String = data.Split(Environment.NewLine)
        Dim settings As Dictionary(Of String, String) = New Dictionary(Of String, String)
        For Each line As String In lines
            Dim parts() As String = line.Split("=".ToCharArray(), 2)
            If Not parts.Length <> 2 Then
                ' we have a key value pair
                If Not settings.ContainsKey(parts(0).Trim()) Then
                    settings.Add(parts(0).Trim(), parts(1))
                Else
                    settings.Item(parts(0)) = parts(1)
                End If
            End If
        Next

        ' Default settings
        If Not settings.ContainsKey("Image_Path") Then settings.Add("Image_Path", "")
        If Not settings.ContainsKey("Image_Size") Then settings.Add("Image_Size", "1280x720")
        If Not settings.ContainsKey("Image_Start") Then settings.Add("Image_Start", "1")
        If Not settings.ContainsKey("Image_Stop") Then settings.Add("Image_Stop", "20")
        If Not settings.ContainsKey("Image_Prev") Then settings.Add("Image_Prev", "1")

        If Not settings.ContainsKey("Font_Family") Then settings.Add("Font_Family", "Arial")
        If Not settings.ContainsKey("Font_Size") Then settings.Add("Font_Size", "192")
        If Not settings.ContainsKey("Font_Style") Then settings.Add("Font_Style", "False, False, False, False")
        If Not settings.ContainsKey("Font_Loc") Then settings.Add("Font_Loc", "Bottom Left")
        If Not settings.ContainsKey("Font_Color") Then settings.Add("Font_Color", "FFFFFF")

        If Not settings.ContainsKey("Save_Loc") Then settings.Add("Save_Loc", "")
        If Not settings.ContainsKey("Save_File") Then settings.Add("Save_File", "Ep %i.bmp")
        If Not settings.ContainsKey("Save_Format") Then settings.Add("Save_Format", "BMP")

        ' Update application with setting data
        ' Image Tab
        txtFile.Text = settings.Item("Image_Path")
        nudImageWidth.Value = Integer.Parse(settings.Item("Image_Size").Split("x")(0))
        nudImageHeight.Value = Integer.Parse(settings.Item("Image_Size").Split("x")(1))
        nudEpStart.Value = Integer.Parse(settings.Item("Image_Start"))
        nudEpStop.Value = Integer.Parse(settings.Item("Image_Stop"))
        nudEpPrev.Value = Integer.Parse(settings.Item("Image_Prev"))

        ' Font Tab
        cbFontName.Text = settings.Item("Font_Family")
        nudFontSize.Value = Integer.Parse(settings.Item("Font_Size"), 192)
        chkFontBold.Checked = Boolean.Parse(settings.Item("Font_Style").Split(", ")(0))
        chkFontItalic.Checked = Boolean.Parse(settings.Item("Font_Style").Split(", ")(1))
        chkFontUnderline.Checked = Boolean.Parse(settings.Item("Font_Style").Split(", ")(2))
        chkFontStrike.Checked = Boolean.Parse(settings.Item("Font_Style").Split(", ")(3))
        cbFontLocation.Text = settings.Item("Font_Loc")
        txtColorHex.Text = settings.Item("Font_Color")
        txtColorHex_TextChanged(Nothing, Nothing)  ' Force color info to be set from load

        ' Control Tab
        txtSaveLoc.Text = settings.Item("Save_Loc")
        txtSaveFile.Text = settings.Item("Save_File")
        cbFormat.Text = settings.Item("Save_Format")

        updateEpFont()
        updatePreview()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Dim ret As DialogResult = MessageBox.Show("Are you sure you wish to exit?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
        If Not ret = DialogResult.No Then
            Application.Exit()
        End If
    End Sub

    Private Sub cbQuickSet_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbQuickSet.SelectedIndexChanged
        Dim sizeParts() As String = cbQuickSet.Text.Split("x")
        Dim width As Integer = Integer.Parse(sizeParts(0))
        Dim height As Integer = Integer.Parse(sizeParts(1))
        If width = 0 Or height = 0 Then Return

        frmBmp.Width = width
        frmBmp.Height = height
        nudImageWidth.Value = width
        nudImageHeight.Value = height

        ' Console.WriteLine(width & "x" & height & " ::: " & cbQuickSet.Text & " ::: " & sizeParts(0) & " ::: " & sizeParts(1))
    End Sub

    Private Sub nudEpStart_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudEpStart.ValueChanged
        nudEpStop.Minimum = nudEpStart.Value
    End Sub

    Private Sub nudEpStop_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudEpStop.ValueChanged
        nudEpStart.Maximum = nudEpStop.Value
    End Sub

    Private Sub btnSaveLoc_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveLoc.Click
        fbd.Description = "Select folder to save rendered images to."
        fbd.ShowNewFolderButton = True
        Dim ret As DialogResult = fbd.ShowDialog()
        If Not ret = Windows.Forms.DialogResult.Cancel Then
            txtSaveLoc.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub btnCreate_Click(sender As System.Object, e As System.EventArgs) Handles btnCreate.Click
        pbCreate.Value = 0
        pbCreate.Maximum = nudEpStop.Value - nudEpStart.Value + 1
        pbCreate.Minimum = 0

        If txtSaveLoc.Text = "" Then
            MessageBox.Show("No save location provided!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If txtSaveFile.Text = "" Then
            MessageBox.Show("No file name provided!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim bmp As New Bitmap(frmBmp.Width, frmBmp.Height, frmBmp.CreateGraphics())
        For i = nudEpStart.Value To nudEpStop.Value
            pbCreate.Value = i - nudEpStart.Value + 1
            frmBmp.lblEp.Text = i

            frmBmp.DrawToBitmap(bmp, New Rectangle(0, 0, frmBmp.Width, frmBmp.Height))

            Select Case cbFormat.Text
                Case "BMP"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Bmp)
                Case "EMF"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Emf)
                Case "EXIF"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Exif)
                Case "GIF"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Gif)
                Case "ICON"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Icon)
                Case "JPEG"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Jpeg)
                Case "PNG"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Png)
                Case "TIFF"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Tiff)
                Case "WMF"
                    bmp.Save(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i), System.Drawing.Imaging.ImageFormat.Wmf)
            End Select

            ' Read last saved file size
            Dim fi As New System.IO.FileInfo(txtSaveLoc.Text & "\" & txtSaveFile.Text.Replace("%i", i))
            If fi.Length > 2 * 1024 * 1024 Then ' 2MB

            End If
        Next

        MessageBox.Show("Created " & (nudEpStop.Value - nudEpStart.Value + 1) & " thumbnail images.", "Task Finished", MessageBoxButtons.OK)
    End Sub

    Private Sub VersionToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles VersionToolStripMenuItem.Click
        MessageBox.Show("Version: " & Application.ProductVersion, "Thumbnail Maker Version", MessageBoxButtons.OK, MessageBoxIcon.None)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show("Thumbnail Maker developed by Kalbintion" & Environment.NewLine & Environment.NewLine & "Copyright 2018 Kalbintion. All Rights Reserved.", "Thumbnail Maker About", MessageBoxButtons.OK, MessageBoxIcon.None)
    End Sub

    Private Sub nudEpPrev_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudEpPrev.ValueChanged
        frmBmp.lblEp.Text = nudEpPrev.Value
    End Sub

    Private Sub btnRegCreate_Click(sender As System.Object, e As System.EventArgs) Handles btnRegCreate.Click
        If FileAssoc.CreateAssoc(".ktm", "KTMDoc", "Thumbnail Maker File", Application.ExecutablePath) Then
            FileAssocCheck()
        Else
            MessageBox.Show("Failed to create file association! Make sure the application was ran using administrator priviledges.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnRegRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRegRemove.Click
        If FileAssoc.RemoveAssoc(".ktm", "KTMDoc") Then
            FileAssocCheck()
        Else
            MessageBox.Show("Failed to remove file association! Make sure the application was ran using administrator priviledges.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub FileAssocCheck()
        If FileAssoc.AssocExists(".ktm", "KTMDoc") Then
            btnRegCreate.Enabled = False
            btnRegRemove.Enabled = True
        Else
            btnRegCreate.Enabled = True
            btnRegRemove.Enabled = False
        End If
    End Sub

    Private Sub cbFormat_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbFormat.SelectedIndexChanged

    End Sub
End Class
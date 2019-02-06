Public Class frmBmp

    Private Sub frmBmp_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lblEp.Parent = pbImg

        ' generateImages()
    End Sub

    Private Sub generateImages()
        Dim bmp As New Bitmap(Me.Width, Me.Height, Me.CreateGraphics())
        For i = 1 To 12
            lblEp.Text = i
            Me.DrawToBitmap(bmp, New Rectangle(0, 0, Me.Width, Me.Height))
            bmp.Save("Z:\Saves\DDN E" & i & ".bmp")
        Next
    End Sub
End Class

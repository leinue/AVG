Public Class Form1
    Dim Control As OAECtrl

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Control.Init(New Form1())
    End Sub
End Class

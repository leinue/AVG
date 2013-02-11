Public Class Form1
    Dim gameForm As Form = New Form
    Dim gameControl As OAECtrl = New OAECtrl

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Debug.WriteLine("Hello")
        gameControl.Init(gameForm)
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    End Sub
End Class

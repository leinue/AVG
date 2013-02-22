Imports System.Drawing.Imaging

Public Class OAEDisplay
    Public IgnoreError As Boolean = False

    Public ItemList() As OAEItem
    Public GameForm As Form
    Dim CacheBmp As Bitmap
    Dim BmpGrap As Graphics
    Dim FormGrap As Graphics

    Sub New(ByRef Form As Form)
        GameForm = Form

        AddHandler GameForm.FormClosing, AddressOf Dispose
        AddHandler GameForm.Paint, AddressOf fPaintForm
        AddHandler GameForm.MouseMove, AddressOf eMouseMove
        AddHandler GameForm.MouseDown, AddressOf eMouseDown
    End Sub

    Sub bDrawItem(ByRef Item As OAEItem)
        If Item.Type = "Text" Then
            Dim Text As OAEItemText
            If Item.Status = "Normal" Then
                Text = Item.Text.NormalText
            ElseIf Item.Status = "Hover" Then
                Text = Item.Text.HoverText
            ElseIf Item.Status = "Click" Then
                Text = Item.Text.ClickText
            Else
                If IgnoreError = False Then Throw New Exception("(bDrawItem)Illegal Status:" + Item.Status)
                Text = New OAEItemText
            End If

            If Text.Text = "" Then
                Text.Text = Item.Text.NormalText.Text
            End If
            If Text.Font.Font Is Nothing Then
                Text.Font.Font = Item.Text.NormalText.Font.Font
            End If
            If Text.Font.Brush Is Nothing Then
                Text.Font.Brush = Item.Text.NormalText.Font.Brush
            End If

                DrawText(BmpGrap, Text, Item.Position)
            End If


        If Item.Type = "Image" Then
            Dim Image As OAEItemImage
            If Item.Status = "Normal" Then
                Image = Item.Image.NormalImage
            ElseIf Item.Status = "Hover" Then
                Image = Item.Image.HoverImage
            ElseIf Item.Status = "Click" Then
                Image = Item.Image.ClickImage
            Else
                If IgnoreError = False Then Throw New Exception("(bDrawItem)Illegal Status:" + Item.Status)
                Image = New OAEItemImage
            End If

            If Image.Image Is Nothing Then
                Image.Image = Item.Image.NormalImage.Image
            End If

            DrawImage(BmpGrap, Image, Item.Position)
        End If
    End Sub

    Sub DrawText(ByRef Grap As Graphics, ByRef Text As OAEItemText, ByVal range As Rectangle)
        Grap.DrawString(Text.Text, Text.Font.Font, Text.Font.Brush, range)

        If Text.Effect.Shadow.Enable = True Then
            Grap.DrawString(Text.Text, Text.Effect.Shadow.Font.Font, Text.Effect.Shadow.Font.Brush, New Rectangle(range.X + Text.Effect.Shadow.Offset, range.Y + Text.Effect.Shadow.Offset, range.Width, range.Height))
        End If
    End Sub

    Sub DrawImage(ByRef Grap As Graphics, ByRef Image As OAEItemImage, ByVal range As Rectangle)
        If Image.Effect.Transparent.Enable = True Then
            Dim ImageAttrs As ImageAttributes

        End If
    End Sub

    Function ImageApplyTransp(ByVal Transparent As Integer) As ImageAttributes
        If Transparent > 225 Then
            If IgnoreError = False Then Throw New Exception("(ImageApplyTransp)Illegal Transparent:" + Transparent)
        End If
        Dim matrixItems As Single()() = { _
           New Single() {1, 0, 0, 0, 0}, _
           New Single() {0, 1, 0, 0, 0}, _
           New Single() {0, 0, 1, 0, 0}, _
           New Single() {0, 0, 0, CDbl(Transparent) / 225.0F, 0}, _
           New Single() {0, 0, 0, 0, 1}}

        Dim colorMatrix As New ColorMatrix(matrixItems)
        Dim imageAttr As New ImageAttributes()
        imageAttr.SetColorMatrix( _
           colorMatrix, _
           ColorMatrixFlag.Default, _
           ColorAdjustType.Bitmap)

        Return imageAttr
    End Function
End Class

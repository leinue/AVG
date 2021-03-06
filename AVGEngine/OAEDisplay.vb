﻿Imports System.Drawing.Imaging

Public Class OAEDisplay
    Public IgnoreError As Boolean = False

    Public ItemList(1) As OAEItem
    Public GameForm As Form
    Dim CacheBmp As Bitmap
    Dim BmpGrap As Graphics
    Dim FormGrap As Graphics

    Sub New(ByRef Form As Form)
        GameForm = Form

        AddHandler GameForm.FormClosing, AddressOf Dispose
        AddHandler GameForm.Paint, AddressOf PaintForm
        AddHandler GameForm.MouseMove, AddressOf eMouseMove
        AddHandler GameForm.MouseDown, AddressOf eMouseDown
    End Sub

    Public Sub Init(ByVal Width As Integer, ByVal Height As Integer)
        GameForm.Height = Height
        GameForm.Width = Width

        ResetItemList()

        CacheBmp = New Bitmap(Width, Height)
        BmpGrap = Graphics.FromImage(CacheBmp)
        FormGrap = Graphics.FromHwnd(GameForm.Handle)
    End Sub

    Private Sub DrawItem(ByRef Grap As Graphics, ByRef Item As OAEItem)
        If Item.Type = "Text" Then
            Dim Text As OAEItemText
            If Item.Status = "Normal" Then
                Text = Item.Text.NormalText
            ElseIf Item.Status = "Hover" Then
                Text = Item.Text.HoverText
            ElseIf Item.Status = "Click" Then
                Text = Item.Text.ClickText
            Else
                If IgnoreError = False Then Throw New Exception("(OAEDisplay)(DrawItem)Illegal Status:" + Item.Status)
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

            DrawText(Grap, Text, Item.Position)
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
                If IgnoreError = False Then Throw New Exception("(OAEDisplay)(DrawItem)Illegal Status:" + Item.Status)
                Image = New OAEItemImage
            End If

            If Image.Image Is Nothing Then
                Image.Image = Item.Image.NormalImage.Image
            End If

            DrawImage(Grap, Image, Item.Position)
        End If
    End Sub

    Private Sub DrawText(ByRef Grap As Graphics, ByRef Text As OAEItemText, ByVal range As Rectangle)
        Grap.DrawString(Text.Text, Text.Font.Font, Text.Font.Brush, range)

        If Text.Effect.Shadow.Enable = True Then
            Grap.DrawString(Text.Text, Text.Effect.Shadow.Font.Font, Text.Effect.Shadow.Font.Brush, New Rectangle(range.X + Text.Effect.Shadow.Offset, range.Y + Text.Effect.Shadow.Offset, range.Width, range.Height))
        End If
    End Sub

    Private Sub DrawImage(ByRef Grap As Graphics, ByRef Image As OAEItemImage, ByVal range As Rectangle)
        If Image.Effect.Transparent.Enable = True Then
            Dim ImageAtt As ImageAttributes = ImageApplyTransp(Image.Effect.Transparent.Transparent)
            Grap.DrawImage(Image.Image, range, 0, 0, Image.Image.Width, Image.Image.Height, GraphicsUnit.Pixel, ImageAtt)
        Else
            Grap.DrawImage(Image.Image, range)
        End If
    End Sub

    Private Function ImageApplyTransp(ByVal Transparent As Integer) As ImageAttributes
        If Transparent > 225 Then
            If IgnoreError = False Then Throw New Exception("(OAEDisplay)(ImageApplyTransp)Illegal Transparent:" + Transparent)
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

    Sub eMouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        For i As Integer = 0 To UBound(ItemList)
            If InRectangle(New Point(e.X, e.Y), ItemList(i).Position) Then
                If ItemList(i).Status <> "Hover" Then
                    ItemList(i).Status = "Hover"
                    EventOccur(ItemList(i), "Hover")
                    PaintForm()
                End If
            Else
                If ItemList(i).Status <> "Normal" Then
                    ItemList(i).Status = "Normal"
                    PaintForm()
                End If
            End If
        Next
    End Sub

    Sub eMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        For i As Integer = 0 To UBound(ItemList)
            If InRectangle(New Point(e.X, e.Y), ItemList(i).Position) Then
                If ItemList(i).Status <> "Click" Then
                    ItemList(i).Status = "Click"
                    EventOccur(ItemList(i), "Click")
                    PaintForm()
                End If
            Else
                If ItemList(i).Status <> "Normal" Then
                    ItemList(i).Status = "Normal"
                    PaintForm()
                End If
            End If
        Next
    End Sub

    Private Function InRectangle(ByVal Point As Point, ByVal Rectangle As Rectangle) As Boolean
        If Rectangle.X < Point.X And _
            Point.X < (Rectangle.X + Rectangle.Width) And _
            Rectangle.Y < Point.Y And _
            Point.Y < (Rectangle.Y + Rectangle.Height) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub ResetItemList()
        For i As Integer = 0 To UBound(ItemList)
            ItemList(i).Dispose()
        Next

        ReDim ItemList(0)
    End Sub

    Public Sub PaintForm()
        For i As Integer = 0 To UBound(ItemList)
            If ItemList(i).Available = True And ItemList(i).Visible = True Then
                DrawItem(BmpGrap, ItemList(i))
            End If
        Next

        FormGrap.DrawImage(CacheBmp, 0, 0)
    End Sub

    Public Sub AddItem(ByRef Item As OAEItem)
        For j As Integer = 0 To UBound(ItemList)
            If ItemList(j).Name = Item.Name Then
                ItemList(j) = Item
            End If
        Next

        Dim i As Integer = 0
        While i <= UBound(ItemList)
            If ItemList(i).Available = False Then
                ItemList(i) = Item
                Return
            End If

            i = i + 1
        End While

        ReDim Preserve ItemList(2 * ItemList.Length - 1)

        ItemList(i) = Item
    End Sub

    Public Sub DeleteItem(ByVal Name As String)
        For j As Integer = 0 To UBound(ItemList)
            If ItemList(j).Name = Name Then
                ItemList(j).Dispose()
                Return
            End If
        Next

        If IgnoreError = False Then Throw New Exception("(OAEDisplay)(ImageApplyTransp)Try to delete an nonexistent item:" + Name)
    End Sub

    Public Sub Dispose()
        ResetItemList()

        CacheBmp.Dispose()
        BmpGrap.Dispose()
        FormGrap.Dispose()
    End Sub

    Private Sub EventOccur(ByRef Item As OAEItem, ByVal EventType As String)
        If EventType = "Hover" Then
            Item.EventCallArgs.InvokeFunction.Invoke(Item.EventCallArgs.OnHover)
        ElseIf EventType = "Click" Then
            Item.EventCallArgs.InvokeFunction.Invoke(Item.EventCallArgs.OnClick)
        End If
    End Sub
End Class

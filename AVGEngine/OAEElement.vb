Public Class OAEItem
    Public Name As String
    Public Type As String
    Public Status As String
    Public Position As Rectangle
    Public Visible As Boolean
    Public Text As OAETextItem
    Public Image As OAEImageItem
    Public EventCallArgs As OAEEventCallArgs

    Public Sub Dispose()
        Image.HoverImage.Image.Dispose()
        Image.NormalImage.Image.Dispose()
        Image.ClickImage.Image.Dispose()
        Text.NormalText.Font.Font.Dispose()
        Text.NormalText.Font.Brush.Dispose()
        Text.HoverText.Font.Font.Dispose()
        Text.HoverText.Font.Brush.Dispose()
        Text.ClickText.Font.Font.Dispose()
        Text.ClickText.Font.Brush.Dispose()
    End Sub
End Class

Public Structure OAETextItem
    Dim NormalText As OAEItemText
    Dim HoverText As OAEItemText
    Dim ClickText As OAEItemText
End Structure

Public Structure OAEItemText
    Dim Text As String
    Dim Font As OAEFont
    Dim Effect As OAETextEffect
End Structure

Public Structure OAEImageItem
    Dim NormalImage As OAEItemImage
    Dim HoverImage As OAEItemImage
    Dim ClickImage As OAEItemImage
End Structure

Public Structure OAEItemImage
    Dim Image As Image
    Dim Effect As OAEImageEffect
End Structure

Public Delegate Sub EventReceive(ByVal EventArgs As String)

Public Structure OAEEventCallArgs
    Dim InvokeFunction As EventReceive
    Dim OnClick As String
    Dim OnHover As String
End Structure

Public Structure OAEFont
    Dim Font As Font
    Dim Brush As SolidBrush
End Structure

Public Structure OAETextEffect
    Dim Shadow As OAEEffectShadow
End Structure

Public Structure OAEImageEffect
    Dim Shadow As OAEEffectShadow
    Dim Transparent As OAEEffectTransp
End Structure

Public Structure OAEEffectShadow
    Dim Enable As Boolean
    Dim Offset As Integer
    Dim Font As OAEFont
End Structure

Public Structure OAEEffectTransp
    Dim Enable As Boolean
    Dim Transparent As Integer
End Structure
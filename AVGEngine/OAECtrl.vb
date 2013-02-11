Public Class OAECtrl

    Const IGNORE_ERROR As Boolean = False

    '---------Struct---------
    Structure InDisplayItem
        Dim Item As OAEItem
        Dim ItemStatus As String 'Such as 'Normal','Hover','Click',etc.
        Dim LastDrawStatus As String 'Status of the item during the lastest drawing process.
    End Structure

    Structure ImageInfo
        Dim Image As Image
        Dim ID As String 'A tag to describe this image.
    End Structure

    Structure FontInfo
        Dim Font As OAEFont
        Dim ID As String 'The same to above
    End Structure

    Structure OAEFont
        Dim Font As Font
        Dim Shadow As OAEShadow
        Dim Brush As SolidBrush
    End Structure

    Structure OAEShadow
        Dim EnableShawdow As Boolean
        Dim ShadowOffset As Integer
    End Structure

    '---------Var---------
    Dim ScriptFilePath As String = Application.StartupPath + "script\script.ini"
    Dim ScriptI As OAEScriptEngine
    Dim InitInfo As OAEInitInfo
    Dim MusicPlayer As System.Media.SoundPlayer
    Dim gForm As Form 'The form to draw.
    Dim ItemList() As InDisplayItem 'A list of the items which are displaying on the window.
    Dim imageList() As ImageInfo 'All Image resources.
    Dim fontList() As FontInfo
    Dim g As Graphics

    '---------Internal Function---------
    Public Sub Init(ByVal ScriptFile As String, ByVal GameForm As Form)
        gForm = GameForm

        AddHandler gForm.FormClosing, AddressOf Destory

        ScriptI = New OAEScriptEngine(ScriptFilePath)
        InitInfo = ScriptI.GetInitInfo()
        If InitInfo.width > 0 Then
            gForm.Width = InitInfo.width
        End If
        If InitInfo.height > 0 Then
            gForm.Height = InitInfo.height
        End If

        g = Graphics.FromHwnd(gForm.Handle)

        ShowWindow("Main")

    End Sub

    Sub ShowWindow(ByVal WindowName As String)
        Dim Window As OAEWindow = ScriptI.GetWindow(WindowName)
        Dim ItemNameList As String()
        If Window.bgMusic <> "" Then
            MusicPlayer.SoundLocation = Window.bgMusic
            MusicPlayer.PlayLooping()
        End If
        If Window.bgImage <> "" Then
            gForm.BackgroundImage = System.Drawing.Image.FromFile(Window.bgImage)
        End If
        If Window.itemList <> "" Then
            ItemNameList = Window.itemList.Split(",")

            ReDim ItemList(UBound(ItemNameList))

            For i As Integer = 0 To UBound(ItemList)
                ItemList(i).Item = ScriptI.GetItem(ItemNameList(i))
                ItemList(i).ItemStatus = "Normal"
                ItemList(i).LastDrawStatus = ""
                DrawItem(ItemList(i))
            Next

        End If
    End Sub

    '---------Function About Drawing---------

    Sub DrawItem(ByVal Item As InDisplayItem)
        If Item.Item.type = "Image" Then
            DrawImage(Item)
        ElseIf Item.Item.type = "Text" Then
            DrawText(Item)
        End If

    End Sub

    Sub DrawText(ByVal Item As InDisplayItem)
        Dim mFont As OAEFont = GetItemFont(Item)
        Dim range As Rectangle = GetItemTextRange(Item)
        g.DrawString(GetItemText(Item), mFont.Font, mFont.Brush, range)
    End Sub

    Sub DrawImage(ByVal Item As InDisplayItem)
        Dim image As Image = GetItemImage(Item)

        g.DrawImage(image, Item.Item.locX, Item.Item.locY, Item.Item.locX + image.Width, Item.Item.locY + image.Width)

    End Sub

    '---------Function About Image---------

    Function GetItemImage(ByVal Item As InDisplayItem) As Image
        For i As Integer = 0 To UBound(imageList)
            imageList(i).ID = Item.Item.name + "-" + Item.ItemStatus
            Return imageList(i).Image
        Next

        If UBound(imageList) - LBound(imageList) = 0 Then
            ReDim Preserve imageList(2 * UBound(imageList))
        End If

        If Item.ItemStatus = "Normal" Then

            imageList(LBound(imageList)).Image = ScriptI.GetImageRes(Item.Item.NormalImage)
            imageList(LBound(imageList)).ID = Item.Item.name + "-" + "Normal"

            Return imageList(LBound(imageList)).Image
        ElseIf Item.ItemStatus = "Hover" Then
            imageList(LBound(imageList)).Image = ScriptI.GetImageRes(Item.Item.HoverImage)
            imageList(LBound(imageList)).ID = Item.Item.name + "-" + "Hover"
            Return imageList(LBound(imageList)).Image
        ElseIf Item.ItemStatus = "Click" Then
            imageList(LBound(imageList)).Image = ScriptI.GetImageRes(Item.Item.ClickImage)
            imageList(LBound(imageList)).ID = Item.Item.name + "-" + "Click"
            Return imageList(LBound(imageList)).Image
        End If

        If IGNORE_ERROR = False Then Throw New Exception("Unknow event type : " + Item.ItemStatus)

        Return Nothing
    End Function

    '---------Function About Font&Text---------
    Function GetFont(ByVal Fontcodes As String) As OAEFont
        Dim FontCode() As String = Fontcodes.Split(";")
        Dim tempAttr() As String
        Dim Attrs() As String = {"Verdana", "13", "Regular", "Black", "Disable", "2", "255"} 'Font family;Size;Style;Color;Shadow;ShadowOffset;Transparent

        For i As Integer = 0 To UBound(FontCode)
            tempAttr = FontCode(i).Split(":")

            tempAttr(0) = tempAttr(0).Trim()

            If tempAttr(0) = "family" Then
                Attrs(0) = tempAttr(1).Trim()
            ElseIf tempAttr(0) = "size" Then
                Attrs(1) = tempAttr(1).Trim()
            ElseIf tempAttr(0) = "style" Then
                Attrs(2) = tempAttr(1).Trim()
            ElseIf tempAttr(0) = "color" Then
                Attrs(3) = tempAttr(1).Trim()
            ElseIf tempAttr(0) = "shadow" Then
                Dim shadow() As String = tempAttr(1).Split(",")
                Attrs(4) = shadow(0).Trim()
                Attrs(5) = shadow(1).Trim()
            ElseIf tempAttr(0) = "transparent" Then
                Attrs(6) = tempAttr(1).Trim()
            Else
                If IGNORE_ERROR = False Then Throw New Exception("Unknow fontcode Attr : " + tempAttr(0))
            End If

        Next

        GetFont.Font = New Font(Attrs(0), CInt(Attrs(1)), GetFontStyle(Attrs(2)))
        If Attrs(4) = "Enable" Then
            GetFont.Shadow.EnableShawdow = True
        Else
            GetFont.Shadow.EnableShawdow = False
        End If
        GetFont.Shadow.ShadowOffset = CInt(Attrs(5))
        GetFont.Brush = New SolidBrush(GetColorFromCode(Attrs(3), CInt(Attrs(6))))

    End Function

    Function GetColorFromCode(ByVal code As String, ByVal transparent As Integer) As Color
        code = code.Trim()
        If code.StartsWith("#") Then
            Return Color.FromArgb(transparent, Convert.ToInt32(code.Substring(1, 2), 16), Convert.ToInt32(code.Substring(3, 2), 16), Convert.ToInt32(code.Substring(5, 2), 16))
        Else
            Dim mColor As Color = Color.FromName(code)
            Return Color.FromArgb(transparent, mColor)
            Return mColor
        End If
    End Function

    Function GetFontStyle(ByVal Style As String) As FontStyle
        If Style = "Regular" Then
            Return FontStyle.Regular
        ElseIf Style = "Bold" Then
            Return FontStyle.Bold
        ElseIf Style = "Italic" Then
            Return FontStyle.Italic
        ElseIf Style = "Strikeout" Then
            Return FontStyle.Strikeout
        ElseIf Style = "Underline" Then
            Return FontStyle.Underline
        End If

        If IGNORE_ERROR = False Then Throw New Exception("Unknow fontstyle : " + Style)

        Return Nothing
    End Function

    Function GetItemFont(ByVal Item As InDisplayItem) As OAEFont
        For i As Integer = 0 To UBound(fontList)
            fontList(i).ID = Item.Item.name + "-" + Item.ItemStatus
            Return fontList(i).Font
        Next

        If UBound(fontList) - LBound(fontList) = 0 Then
            ReDim Preserve fontList(2 * UBound(fontList))
        End If

        If Item.ItemStatus = "Normal" Then
            fontList(LBound(fontList)).Font = GetFont(Item.Item.NormalFont)
            fontList(LBound(fontList)).ID = Item.Item.name + "-" + "Normal"
            Return fontList(LBound(fontList)).Font
        ElseIf Item.ItemStatus = "Hover" Then
            fontList(LBound(fontList)).Font = GetFont(Item.Item.NormalFont)
            fontList(LBound(fontList)).ID = Item.Item.name + "-" + "Hover"
            Return fontList(LBound(fontList)).Font
        ElseIf Item.ItemStatus = "Click" Then
            fontList(LBound(fontList)).Font = GetFont(Item.Item.NormalFont)
            fontList(LBound(fontList)).ID = Item.Item.name + "-" + "Click"
            Return fontList(LBound(fontList)).Font
        End If

        If IGNORE_ERROR = False Then Throw New Exception("Unknow event type : " + Item.ItemStatus)

        Return Nothing
    End Function

    Function GetItemText(ByVal Item As InDisplayItem) As String
        Dim wChar() As Char = {" ", Chr(34)}
        If Item.ItemStatus = "Normal" Then
            Return Item.Item.NormalText.Trim(wChar)
        ElseIf Item.ItemStatus = "Hover" Then
            Return Item.Item.HoverText.Trim(wChar)
        ElseIf Item.ItemStatus = "Click" Then
            Return Item.Item.ClickText.Trim(wChar)
        End If

        If IGNORE_ERROR = False Then Throw New Exception("Unknow event type : " + Item.ItemStatus)

        Return Nothing
    End Function

    '---------Window Event Process Function---------
    Sub Destory()
        g.Dispose()
        For i As Integer = 0 To UBound(imageList)
            imageList(i).Image.Dispose()
        Next

    End Sub

End Class
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
    Dim ScriptFilePath As String = Application.StartupPath + "//script//script.ini"
    Dim ScriptI As OAEScriptEngine ' Script describer.
    Dim InitInfo As OAEInitInfo
    Dim MusicPlayer As System.Media.SoundPlayer = New System.Media.SoundPlayer ' To play sound.
    Dim gForm As Form 'The main game form to draw.
    Dim ItemList() As InDisplayItem 'A list of the items which are displaying on the window.
    Dim imageList() As ImageInfo 'Cache Image resources. Clean while changing window.
    Dim fontList() As FontInfo 'Cache Font.

    Dim CacheBmp As Bitmap
    Dim g As Graphics ' Cache Graphics
    Dim gFormGra As Graphics ' Real Graphics

    '---------Internal Function---------
    Public Sub Init(ByVal GameForm As Form)
        gForm = GameForm

        RegEvent() 'Register event to function here.

        ScriptI = New OAEScriptEngine(ScriptFilePath)
        InitInfo = ScriptI.GetInitInfo()

        ReDim imageList(0), fontList(0)

        If InitInfo.width > 0 And InitInfo.height > 0 Then
            gForm.ClientSize = New Size(InitInfo.width, InitInfo.height)
        Else
            gForm.ClientSize = New Size(800, 600)
        End If
        

        CacheBmp = New Bitmap(gForm.Width, gForm.Height)
        g = Graphics.FromImage(CacheBmp)
        gFormGra = Graphics.FromHwnd(gForm.Handle) ' Init GDI+

        ShowScene("Main") 'All games start with the window Main.
        gForm.Show()

    End Sub

    Sub ShowScene(ByVal SceneName As String)
        Dim Scene As OAEScene = ScriptI.GetScene(SceneName)
        Dim ItemNameList() As String

        If Scene.bgMusic <> "" Then
            MusicPlayer.SoundLocation = Scene.bgMusic ' **BUG: NullRef
            MusicPlayer.PlayLooping()

        End If

        If Scene.itemList <> "" Then 'Init Items.
            ItemNameList = Scene.itemList.Split(",")

            ReDim ItemList(UBound(ItemNameList))

            For i As Integer = 0 To UBound(ItemList)
                ItemList(i).Item = ScriptI.GetItem(ItemNameList(i)) ' Get Item Attributes.
                ItemList(i).ItemStatus = "Normal"
                ItemList(i).LastDrawStatus = ""
            Next

        End If
    End Sub

    '---------Function About Drawing---------

    Sub DrawItem(ByRef Item As InDisplayItem)
        If Item.Item.type = "image" Then
            DrawImage(Item)
        ElseIf Item.Item.type = "text" Then
            DrawText(Item)
        End If

        Item.LastDrawStatus = Item.ItemStatus

    End Sub

    Sub DrawText(ByRef Item As InDisplayItem)
        Dim mFont As OAEFont = GetItemFont(Item)
        Dim range As Rectangle = GetItemTextRange(Item)
        g.DrawString(GetItemText(Item), mFont.Font, mFont.Brush, range)
    End Sub

    Sub DrawImage(ByRef Item As InDisplayItem)
        Dim image As Image = GetItemImage(Item)
        'Debug.WriteLine("Drawed")
        g.DrawImage(image, Item.Item.locX, Item.Item.locY, image.Width, image.Height)

        If Item.Item.width = 0 Then
            Item.Item.width = image.Width
        End If
        If Item.Item.height = 0 Then
            Item.Item.height = image.Height
        End If
    End Sub

    '---------Function About Image---------

    Function GetItemImage(ByVal Item As InDisplayItem) As Image
        If (Item.ItemStatus = "Hover" And Item.Item.HoverImage = "") Or (Item.ItemStatus = "Click" And Item.Item.ClickImage = "") Then
            Item.ItemStatus = "Normal"
        End If
        If imageList IsNot Nothing Then
            For i As Integer = 0 To UBound(imageList)
                If imageList(i).ID = Item.Item.name + "-" + Item.ItemStatus Then
                    Return imageList(i).Image
                End If
            Next

            ReDim Preserve imageList(imageList.Length)
        Else
            ReDim Preserve imageList(0)
        End If

        Dim j As Integer = UBound(imageList)

        If Item.ItemStatus = "Normal" Then
            imageList(j).Image = ScriptI.GetImageRes(Item.Item.NormalImage)
            imageList(j).ID = Item.Item.name + "-" + "Normal"

            Return imageList(j).Image
        ElseIf Item.ItemStatus = "Hover" Then
            imageList(j).Image = ScriptI.GetImageRes(Item.Item.HoverImage)
            imageList(j).ID = Item.Item.name + "-" + "Hover"
            Return imageList(j).Image
        ElseIf Item.ItemStatus = "Click" Then
            imageList(j).Image = ScriptI.GetImageRes(Item.Item.ClickImage)
            imageList(j).ID = Item.Item.name + "-" + "Click"
            Return imageList(j).Image
        End If

        If IGNORE_ERROR = False Then Throw New Exception("Unknow event type : " + Item.ItemStatus)

        Return Nothing
    End Function

    '---------Function About Font&Text---------
    Function GetFont(ByVal Fontcodes As String) As OAEFont ' Get font by fontcode in scripts directly.
        Dim FontCode() As String = Fontcodes.Trim("; ").Split(";")
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

    Function GetColorFromCode(ByVal code As String, ByVal transparent As Integer) As Color ' Get color from color code("#xxxxxx" or "red" in the "color:xxx")
        code = code.Trim()
        If code.StartsWith("#") Then ' #000000
            Return Color.FromArgb(transparent, Convert.ToInt32(code.Substring(1, 2), 16), Convert.ToInt32(code.Substring(3, 2), 16), Convert.ToInt32(code.Substring(5, 2), 16))
        Else ' Red
            Dim mColor As Color = Color.FromName(code)
            Return Color.FromArgb(transparent, mColor)
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

    Function GetItemFont(ByRef Item As InDisplayItem) As OAEFont
        If fontList IsNot Nothing Then
            For i As Integer = 0 To UBound(fontList)
                If fontList(i).ID = Item.Item.name + "-" + Item.ItemStatus Then
                    Return fontList(i).Font
                End If
            Next
            ReDim Preserve fontList(fontList.Length)
        Else
            ReDim Preserve fontList(0)
        End If


        Dim j As Integer = UBound(fontList)

        If Item.ItemStatus = "Normal" Then
            fontList(j).Font = GetFont(Item.Item.NormalFont)
            fontList(j).ID = Item.Item.name + "-" + "Normal"
            Return fontList(j).Font
        ElseIf Item.ItemStatus = "Hover" Then
            fontList(j).Font = GetFont(Item.Item.NormalFont)
            fontList(j).ID = Item.Item.name + "-" + "Hover"
            Return fontList(j).Font
        ElseIf Item.ItemStatus = "Click" Then
            fontList(j).Font = GetFont(Item.Item.NormalFont)
            fontList(j).ID = Item.Item.name + "-" + "Click"
            Return fontList(j).Font
        End If

        If IGNORE_ERROR = False Then Throw New Exception("Unknow event type : " + Item.ItemStatus)

        Return Nothing
    End Function

    Function GetItemText(ByRef Item As InDisplayItem) As String ' Getthe text of a item in current item status.
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

    Function GetItemTextRange(ByVal Item As InDisplayItem) As Rectangle
        Return New Rectangle(Item.Item.locX, Item.Item.locY, Item.Item.TextMaxWidth, Item.Item.TextMaxHeight)
    End Function

    '---------Window Event Process Function---------
    Sub RegEvent() 'Register event process function.
        AddHandler gForm.FormClosing, AddressOf Destory
        AddHandler gForm.Paint, AddressOf gForm_Repaint
        AddHandler gForm.MouseMove, AddressOf gForm_MouseMove
        AddHandler gForm.MouseDown, AddressOf gForm_MouseDown
    End Sub

    Sub Destory() 'Release all resources.
        MusicPlayer.Dispose()
        g.Dispose()
        gFormGra.Dispose()
        CacheBmp.Dispose()
        For i As Integer = 0 To UBound(imageList)
            If imageList(i).Image IsNot Nothing Then
                imageList(i).Image.Dispose()
            End If
        Next
        For i As Integer = 0 To UBound(fontList)
            If fontList(i).Font.Font IsNot Nothing And fontList(i).Font.Brush IsNot Nothing Then
                fontList(i).Font.Font.Dispose()
                fontList(i).Font.Brush.Dispose()
            End If
        Next

    End Sub

    Sub gForm_Repaint()

        For i As Integer = 0 To UBound(ItemList)
            DrawItem(ItemList(i))
        Next

        gFormGra.DrawImage(CacheBmp, 0, 0)

    End Sub

    Sub gForm_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        For i As Integer = 0 To UBound(ItemList)
            If e.X > ItemList(i).Item.locX And e.X < ItemList(i).Item.locX + ItemList(i).Item.width And e.Y > ItemList(i).Item.locY And e.Y < ItemList(i).Item.locX + ItemList(i).Item.height Then
                If ItemList(i).LastDrawStatus <> "Hover" Then
                    ItemList(i).ItemStatus = "Hover"
                    gForm_Repaint()
                End If
            Else
                If ItemList(i).LastDrawStatus <> "Normal" Then
                    ItemList(i).ItemStatus = "Normal"
                    gForm_Repaint()
                End If
            End If
        Next

    End Sub

    Sub gForm_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Debug.WriteLine("Position: " + CStr(e.X) + ", " + CStr(e.Y))
        For i As Integer = 0 To UBound(ItemList)
            If e.X > ItemList(i).Item.locX And e.X < ItemList(i).Item.locX + ItemList(i).Item.width And e.Y > ItemList(i).Item.locY And e.Y < ItemList(i).Item.locX + ItemList(i).Item.height Then
                If ItemList(i).LastDrawStatus <> "Click" Then
                    ItemList(i).ItemStatus = "Click"
                    gForm_Repaint()
                End If
            Else
                If ItemList(i).LastDrawStatus <> "Normal" Then
                    ItemList(i).ItemStatus = "Normal"
                    gForm_Repaint()
                End If
            End If
        Next
    End Sub

End Class
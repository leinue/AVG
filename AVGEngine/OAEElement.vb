Public Structure OAEItem
    'General Info
    Dim name As String
    Dim type As String
    Dim visible As Boolean


    'Location Info
    Dim locX As Integer
    Dim locY As Integer
    Dim height As Integer
    Dim width As Integer


    'Action Info
    Dim ClickAction As String
    Dim HoverAction As String


    'Attributes of Image
    Dim NormalImage As String
    Dim HoverImage As String
    Dim ClickImage As String


    'Attributes of Text
    Dim NormalText As String
    Dim HoverText As String
    Dim ClickText As String

    Dim NormalFont As String
    Dim HoverFont As String
    Dim ClickFont As String

    Dim TextMaxWidth As String
    Dim TextMaxHeight As String
End Structure
Public Structure OAEWindow
    Dim name As String
    Dim bgImage As String
    Dim bgMusic As String
    Dim itemList As String
End Structure
Public Structure OAEInitInfo
    Dim width As Integer
    Dim height As Integer
End Structure
Public Structure OAEWindowAttr
    Dim included As String
End Structure
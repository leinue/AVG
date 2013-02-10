﻿Public Structure OAEItem
    'General Info
    Dim name As String
    Dim type As String
    'Location Info
    Dim locX As Integer
    Dim locY As Integer
    Dim height As Integer
    Dim width As Integer
    'Action Info
    Dim ClickAction As String
    Dim HoverAction As String
    'Event of Image
    Dim NormalImage As String
    Dim HoverImage As String
    Dim ClickImage As String
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
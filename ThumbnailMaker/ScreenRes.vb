Imports System.Runtime.InteropServices

Module ScreenRes
    Public Declare Function EnumDisplaySettings Lib "user32" Alias "EnumDisplaySettingsA" (ByVal lpszDeviceName As String, ByVal iModeNum As Integer, ByRef lpdm As DEVMODE) As Boolean
    Public Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Long, ByVal nIndex As Long) As Long

    Public Declare Function ChangeDisplaySettings Lib "user32" (ByRef lpdm As DEVMODE, ByVal iFlags As Integer) As Integer

    Private Const SA_SIZE As Integer = 32
    Private Const DM_BITSPERPEL As Long = &H40000
    Private Const DM_PELSWIDTH As Long = &H80000
    Private Const DM_PELSHEIGHT As Long = &H100000
    Private Const DM_DISPLAYFLAGS As Long = &H200000

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Structure DEVMODE
        Public iSpecVersion As Short
        Public iDriverVersion As Short
        Public iSize As Short
        Public iDriverExtra As Short
        Public iFields As Integer
        Public iOrientation As Short
        Public iPaperSize As Short
        Public iPaperLength As Short
        Public iPaperWidth As Short
        Public iScale As Short
        Public iCopies As Short
        Public iDefaultSource As Short
        Public iPrintQuality As Short
        Public iColor As Short
        Public iDuplex As Short
        Public iYRes As Short
        Public iTTOption As Short
        Public iCollate As Short

        ' marshalling attr...
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=SA_SIZE)> _
        Public lpszFormName As String

        Public iLogPixels As Short
        Public iBitsPerPixel As Integer
        Public lPelsWidth As Integer
        Public lPelsHeight As Integer
        Public lDisplayFlags As Integer
        Public lDisplayFreq As Integer
        Public lICMMethod As Integer
        Public lICMIntent As Integer
        Public lMediaType As Integer
        Public lDitherType As Integer
        Public lReserved1 As Integer
        Public lReserved2 As Integer
        Public lPanWidth As Integer
        Public lPanHeight As Integer

    End Structure

    Public Structure SCREENDATA
        Public width As Integer
        Public height As Integer
        Public freq As Integer
        Public rotation As Integer
        Public color As Integer
    End Structure

    Public Function GetCurrentResolution() As SCREENDATA
        Dim mode As DEVMODE = New DEVMODE
        mode.iSize = CType(Marshal.SizeOf(mode), Short)

        EnumDisplaySettings(Nothing, -1, mode)

        'Console.WriteLine("{0} by {1}, {2} bit, {3} degrees, {4} hertz", mode.lPelsWidth, mode.lPelsHeight, mode.iBitsPerPixel, mode.lDisplayFlags * 90, mode.lDisplayFreq)

        Dim screenData As New SCREENDATA
        screenData.width = mode.lPelsWidth
        screenData.height = mode.lPelsHeight
        screenData.freq = mode.lDisplayFreq
        screenData.rotation = mode.lDisplayFlags * 90
        screenData.color = mode.iBitsPerPixel

        Return screenData

    End Function

    Public Function GetSupportedResolutions() As ArrayList
        Dim supportedRes As New ArrayList
        Dim mode As DEVMODE = New DEVMODE

        mode.iFields = DM_PELSWIDTH Or DM_PELSHEIGHT Or DM_BITSPERPEL
        mode.iSize = CType(Marshal.SizeOf(mode), Short)

        Dim modeIndex As Integer = 0

        ' Console.WriteLine("Supported Modes:")
        While EnumDisplaySettings(Nothing, modeIndex, mode) = True
            ' Console.WriteLine("{0} by {1}, {2} bit, {3} degrees, {4} hertz", mode.lPelsWidth, mode.lPelsHeight, mode.iBitsPerPixel, mode.lDisplayFlags * 90, mode.lDisplayFreq)

            Dim screenData As New SCREENDATA
            screenData.width = mode.lPelsWidth
            screenData.height = mode.lPelsHeight
            screenData.freq = mode.lDisplayFreq
            screenData.rotation = mode.lDisplayFlags * 90
            screenData.color = mode.iBitsPerPixel

            supportedRes.Add(screenData)

            modeIndex += 1
        End While


        Return supportedRes
    End Function
End Module

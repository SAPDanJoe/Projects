Option Explicit

dim x 'declare an uninitialized variable named x
x = 10 'Initialize x to the value 10

'check the underlying data type that was assigned to x upon intialization
Select Case VarType()
	Case vbEmpty
		msgbox "Empty (uninitialized)"
	Case vbNull
		msgbox "Null (no valid data)"
	Case vbInteger
		msgbox "integer"
	Case vbLong
		msgbox "long integer"
	Case vbSingle
		msgbox "single-precision floating-point number"
	Case vbDouble
		msgbox "double-precision floating-point number"
	Case vbCurrency
		msgbox "currency"
	Case vbDate
		msgbox "date"
	Case vbString
		msgbox "string"
	Case vbObject
		msgbox "automation object"
	Case vbError
		msgbox "error"
	Case vbBoolean
		msgbox "boolean"
	Case vbVariant
		msgbox "variant (used only with arrays of Variants)"
	Case vbDataObject
		msgbox "data-access object"
	Case vbByte
		msgbox "byte"
	Case vbArray
		msgbox "array"
End Select

	

ComboBox v1.0 - 8/20/2015

Features:
- Accessable via "GameObject/UI/ComboBox" menu
- List of items will be displayed above the ComboBox, if bottom screen side is too close 
- Can be disabled 
- Configurable amount of elements to display
- First element may be hidden 
- May be configured as multiselect ComboBox 
- Items may contain icons and captions 
- Items may be selected by default 
- Items may be disabled 
- Each item has OnSelect event 
- ComboBox has OnSelect event
- Configurable caption for multiple selected items

UISprite, Background and Font are used for DropDown runtime build.
Interactable enables and disables the control
Padding is applied to caption
Elements to display sets up maximum DropDown height in elements
HideFirst hides first element from DropDown list and selects it by default
FirstIsDefault makes first element selected by default
AllowMultiselect turns ComboBox into multiselect mode
DropDown is shown toggles DropDown
OnSelect allows to subscribe to an event of DropDown hide after any of items were clicked. ComboBox object is passed as an argument
MultiselectCaption allows to set up a function, that provides caption by taking selected items as an argument
Item's caption is text, displayed on this item
Item's icon is icon, displayed on this item
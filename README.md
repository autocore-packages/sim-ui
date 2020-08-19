# sim-ui
UI of Simulator

#1.Create UI

click the button at Asset/CreateSimuUI to Create a GameObject of UI


#2.Set MenuButton

Find the Buttons in folder “SimuUI/PanelTools”  

select the button and find it's Button Component,Edit its onclick attribute to add events

#3.Set SettingPanel

Find the Toggles and sliders in “SimuUI/PanelSettings/PanelInstaction”  

Edit their properties to add events

#4.Set CarMessagePanel

you need to use PanelCarMessage.Instace.UpdateCarmessage to Set the date at Update

Data includes steer(float),Odom(string),brake(float),throttle(float),speed(float),except speed(float)



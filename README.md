This is a WPF control encapsulation designed for Beginner's Guidance, aiming to provide a user-friendly interface that can be applied in various introductory scenarios. The entire codebase is developed using .NET 8 and WPF. It includes three demos, ranging from simple to complex usage of the control, which interested individuals can download and experiment with on their own.

I will first provide an overview of how to use this control and the design idea, followed by an explanation of how the three demos work. I hope this can inspire those developing with WPF and also serve as part of my learning process. If you find the initial section on `Control Encapsulation Introduction` is difficult to understand, I recommend going directly to the demo section. And if you find yourself confused about some features in demos, revisiting the `Control Encapsulation Introduction` when encountering difficulties in the demos might be more efficient.

# Control Encapsulation Introduction
The entire beginner's guide control is encapsulated within the FreshGuidance assembly, primarily encompassing the following aspects:
## GuideMask
This control serves as the overlay for the entire beginner's guide, covering all controls that need to be highlighted in the tutorial. It is typically placed within a Grid and set with the maximum values for Grid.RowSpan and Grid.ColumnSpan to cover the entire page intended to be obscured. The below area in the red rectangle is the area of GuideMask.
![GuideMask is the area in read rectangle](LocalImages/GuideMask.jpg)
## GuideMaskWrapper
GuideMaskWrapper is a wrapper class for GuideMask. This class is necessary because GuideMask is instantiated via a function `static public GuideMask GuideMaskFactory(string key)`, where a key is passed to return a specific instance of GuideMask linked to that key (if the keys are the same, the same GuideMask instance is returned). However, directly invoking `<GuideMask/>` in XAML would create a new instance every time, which does not adhere to the aforementioned functionality. Therefore, GuideMaskWrapper is used to ensure its content is a GuideMask for a specific key. This approach is particularly important for scenarios involving nested XAML calls, like the following example, where guides need to be displayed in both a `Button` and a `Local:UserControl1`; using a key ensures both elements share the same GuideMask instance. But for user's eyes, the area of `GuideMaskWrapper` is the same as the `GuideMask`. If the explanation here is hard to follow, don’t worry,its usage will be elaborated in the third demo.
``` xml
<Window x:Class="GuideMixUsageDemo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:fg="clr-namespace:FreshGuidance;assembly=FreshGuidance"
        xmlns:local="clr-namespace:GuideMixUsageDemo"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="auto"></RowDefinition>
            <RowDefinition Height="auto"></RowDefinition>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="auto"></ColumnDefinition>
            <ColumnDefinition Width="auto"></ColumnDefinition>
        </Grid.ColumnDefinitions>
        <Button Content="A-1" Width="50" Height="50" 
            Grid.Row="0"
            Grid.Column="0"/>
        <local:UserControl1 
            Grid.Row="1" 
            Grid.Column="1" 
             Width="200" Height="200" 
            x:Name="UserControl1">
        </local:UserControl1>
    </Grid>
</Window>
```

## HintControls
The folder `HintControls` contains sub-controls used in the beginner's guide to provide hints, divided into `GuideClickHintControl` and `GuideTipHintControl`.

### GuideClickHintControl
This control adds guidance on a target control to direct user interaction with the target, thereby triggering the target control's functionality and move the guide to the next level. The following picture shows the area of `GuideClickHintControl` in red rectangle, including the blue cricle guiding user to click the button and the mouse picture guiding user to click mouse left button.
![GuideClickHintControl](/LocalImages/GuideClickHintControl.jpg)

### GuideTipHintControl
This control adds guidance on a target control to draw the user's attention to it and provides related hint information. Users can move to the next level of the guide by clicking the `Understand` button. The following picture shows the area of `GuideTipHintControl` in red rectangle.
![GuideTipHintControl](/LocalImages/GuideTipHintControl.jpg)

## HelpClasses
The `HelpClasses` folder contains auxiliary classes that assist with binding target controls, acquiring GuideMasks, and other functions.

### EnumToVisibilityConverter
This class implements `IValueConverter` interface and is used to convert enumeration values into visibility. It is primarily used within the ControlTemplate of `GuideClickHintControl` and `GuideTipHintControl` to toggle the visibility of small directional triangles. This detail may be overlooked without looking at the source code. If you want to look into details, you can check `GuideClickHintControlStyle.xaml` and `GuideTipHintControlStyle.xaml` in `/FreshGuidance/Theme/Styles` directory.

### GetGuideMaskExtension
This class inherits from `MarkupExtension`, and its `ProvideValue` method returns a GuideMask associated with a key by passing in the key. It is used in GuideMaskWrapper to help obtain the GuideMask corresponding to the specified key. The usage is like below, where `fg` is `xmlns:fg="clr-namespace:FreshGuidance;assembly=FreshGuidance"`.
```xml
<fg:GuideMaskWrapper Name="wrapper"
                 Grid.Row="0"
                 Grid.Column="0"
                 Grid.RowSpan="5"
                 Grid.ColumnSpan="5"
                 Content="{fg:GetGuideMask SOME_SPECIFIC_KEY}">
</fg:GuideMaskWrapper>
```

### GuideMaskHelper
This class is a `DependencyObject` and includes an AttachedProperty `HelpBindTargetControl`. This attached property requires a string value, but the actual value isn't as significant. Its primary use is to be applied as an attached property on target controls. Through this mechanism, one can access the target control and some of its related properties.

### SetupGuideMaskContextExtension
This class inherits from `MarkupExtension`. Its `ProvideValue` method returns `string.Empty`, which is primarily used with the attached property `HelpBindTargetControl` of `GuideMaskHelper`. The return value itself is not important; the significance lies in using the attached property to gather information about the desired target controls, such as the control itself, the method that invokes the control, the order of appearance of the guide controls, etc. This concept might be difficult to grasp here but will be detailed in subsequent examples. the usage is like below, where `fg` is `xmlns:fg="clr-namespace:FreshGuidance;assembly=FreshGuidance"`.
```xml
<Button Grid.Row="0" Content="TEST" Width="50" Height="50">
    <fg:GuideMaskHelper.HelpBindTargetControl>
        <fg:SetupGuideMaskContext
                GuideMaskKey="FIRST"                     
                HintControlIndex="0">
            <fg:SetupGuideMaskContext.HintControl>
                <fg:GuideTipHintControl 
                        Background="Bisque"
                        Placement="BOTTOM">
                    <TextBlock Text="Test text test text"/>
                </fg:GuideTipHintControl>
            </fg:SetupGuideMaskContext.HintControl>
        </fg:SetupGuideMaskContext>
    </fg:GuideMaskHelper.HelpBindTargetControl>
</Button>
```
The code snippet above uses `fg:GuideMaskHelper.HelpBindTargetControl` attached property to `Button` and by using `fg:SetupGuideMaskContext` MarkupExtension to set up the `GuideMaskKey`, `HintControlIndex`, and `HintControl` properties. Also `fg:SetupGuideMaskContext` MarkupExtension can get the target control which is `Button`.

# Demo Introduction
There are three demos for showing how to use Beginner's Guidance Assembly.
## GuideTipHintControlDemo
This is the simplest demo. Its main function is to display hint messages on the page. When the user clicks `Understand`, they are guided to the next hint message.

# to be continued...
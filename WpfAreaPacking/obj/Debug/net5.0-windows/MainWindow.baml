   M S B A M L   `   `   `  ����I  EWpfAreaPacking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfAreaPackingWpfAreaPacking  5   	       WpfAreaPacking.MainWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WpfAreaPacking     5   	     � Title$9  3Симулятор размещения в зоне��5   	   $	��650��6-   $	��900��6:   .��5	      �  � RowDefinitions 5
   
   �� 5      $��50 �6   6*   �� 5      $	��500 �6   6+   �� 5      $��50 �6   6*   .+�5   
   ��  �� Orientation$ 	Vertical =�6   $��0��6-   .+�5      �� $ 
Horizontal=�6   $
��20 0q�63   .+�5      �� $��20��6   $	��5 0q�6)   Зона=> длина:6G   �� 5      -   5         ��Name$ 
AreaLength��5      $	��100��6.   $��20��6:   $��36000��6F   �� 5      $��20��6   $	��5 0q�6(   ширина:6?   �� 5      -   5        $ 
AreaHeight��5      $	��100��6.   $��20��6:   $��20000��6F   �� 5      -   5        $��20��5   .   $	��100��6:   .��6F   Обновить6P   �� 5      $��Оптимально:��6   $	��5 0q�6/   $��20��6<   �� 5      -   5         ��Name$ CheckBoxSort��5      $��20��61   �� 5      $#��Инвертировать_X:��6   $	��5 0q�64   $��20��6A   �� 5      -   5        $ CheckBoxInvertX��5      $��20��64   �� 5       $#��Инвертировать_Y:��6   $	��5 0q�64   $��20��6A   �� 5!      -   5        $ CheckBoxInvertY��5!      $��20��64    d� 	IsChecked$
 TrueY�6@   5"      �� 5*      $ 
Horizontal=�6   .+�5+      �� $��20��6   $	��5 0q�6)   
Сдвиг6=   �� 5,      -   5        $ CheckBoxShift��5,      $
 TrueY�62   6E   5-      5.      �� 5/   
   -   5         ��Name$ CanvasBorder��5/      $��1��6   
���   �$��0q�6H   ._�50      �� -	   5        $��MyCanvas��50      51      �� 52   
   -
   5         ��Name$ tbInfo��52   !   $��2��6   $!��Не вместились: ��61   53      54      
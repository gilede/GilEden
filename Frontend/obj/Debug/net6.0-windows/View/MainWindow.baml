   M S B A M L   `   `   `  ����C  ?Frontend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:FrontendFrontend  5   	       Frontend.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Frontend     5   	     � Title$  
MainWindow��5   	   $	��600��6   $
��1000��6)   ��5	      �� 5
   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ 	#FF1F4C43��6    � Offset$ 0Q�6-   � 5      $ 	#FFC2F1E9��6   $ 1Q�6-   5      .��5      � $��30q�6    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ Black��6      	Direction$	 320Q�6-      ShadowDepth$ 2Q�6=   	   
BlurRadius$	 8Q�6M   
   Opacity$	
 0.5Q�6\    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Kanban System��6   $��Center=�6(   $��40(�6E   $
��Bold"�6S   $��Normal&�6e   $��Italic$�6z   ���#FFFFFF$��	0,20,0,20q�6�   � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#    5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5       $
��Auto �6    �� 5!      $
��Auto �6    .+�5#      �� $��Email:��6   $��18(�6%   $��0��63   $��0��6C   $��0,0,10,0q�6P   ���#FFFFFF6b   �� 5$      $��1��6:   $��0��6J   $��35��6W   $��16(�6c   $��0,0,0,10q�6q   ��6   �� *Email+ �� Mode$ TwoWay=�6q   �� 5%      $��	Password:��6   $��18(�6(   $��0��66   $��1��6F   $��0,0,10,0q�6S   ���#FFFFFF6e   �� 5&      $��1��6=   $��1��6M   $��35��6Z   $��16(�6f   $��0,0,0,20q�6t   ��6   �� *
Password+$ TwoWay=�6t   �� 5'      
���  ��$��Center=�6S   $��0��6p   $��2��6�   $��2��6�   ��6   ��  �� Path$ Message�$ OneWay=�6�   �� 5(       �� Orientation$ 
Horizontal=�6   $��3��62   $��0��6?   $��2��6O   $��Center=�6c   .+�5)      �� -   5         ��Name$ Login��5)   )   $��Login��6   $
��20,0q�6M   $��18(�6[   $	��150��6i   $��40��6u   ���	#FF1F4C43
�������6�   �� 5*      -   5        $ Register��5*   ,   $��Register��6   $
��20,0q�6U   $��18(�6c   $	��150��6q   $��40��6}   ���	#FFC2F1E9
���   �6�   5+      5,      5-      5.      
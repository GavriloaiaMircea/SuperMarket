   M S B A M L   `   `   `  ����F  BSupermarket, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Supermarket.ViewsSupermarket.Views  5   	   =#clr-namespace:Supermarket.ViewModelSupermarket.ViewModel  5   	   /    )Supermarket.Views.ProductManagementWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Supermarket.Views     5   	   2vm#clr-namespace:Supermarket.ViewModel     5   	     � Title$  ProductManagementWindow��5	   	   $	��450��6)   $	��800��66   .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��10q�6.   .+�5      �� $��Add Product��6   $	��120��6Q   $��5q�6]   ��6+   �� *AddProductCommand+6]   �� 5      $��Delete Product��6   $	��120��6W   $��5q�6c   ��6.   �� *DeleteProductCommand+6c   �� 5      $��Save Changes��6   $	��120��6S   $��5q�6_   ��6,   �� *SaveChangesCommand+6_   �� 5      $��Refresh��6   $	��100��6'   $��5q�63   ��6>   �� *RefreshCommand+5      &   System.Windows.Controls.DataGrid  5   
      AutoGenerateColumns$ False��64   $��1��6P   $��10q�6]      
IsReadOnly$ False��5   <   ��5      �� *
Products+ �� SelectedItem 5      �� *SelectedProduct+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Product Name��6%      
IsReadOnly$ False��6   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6;   �� *
NumeProdus+
 �� UpdateSourceTrigger$
 PropertyChanged=�6     5      $ Barcode��6%   $ False��6z   	 66   �� *
CodeDeBare+$
 PropertyChanged=�6z     5      $ Category��6%   $
 True��6Y   	 67   �� *NumeCategorie+6Y     5      $ Producer��6%   $
 True��6Z   	 67   �� *NumeProducator+6Z   5      5      5      
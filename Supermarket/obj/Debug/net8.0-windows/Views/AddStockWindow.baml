   M S B A M L   `   `   `  ����F  BSupermarket, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Supermarket.ViewsSupermarket.Views  5   	   =#clr-namespace:Supermarket.ViewModelSupermarket.ViewModel  5   	   &     Supermarket.Views.AddStockWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Supermarket.Views     5   	   2vm#clr-namespace:Supermarket.ViewModel     5   	     � Title$  AddStockWindow��5	   	   $	��500��6    $	��800��6-   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   &   System.Windows.Controls.DataGrid     AutoGenerateColumns$ False��6]   $	��100��6y   $
��0,10q�6�   ��6   �� *
Products+ �� SelectedItem 64   �� *SelectedProduct+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Product Name��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *
NumeProdus+5      �� 5   
   $��	Quantity:��6   $��1��6$   $��Center=�61   $
��0,10q�6L   �� 5   
   -   5         ��Name$ quantityTextBox��5      $��1��6+   $	��300��68   $��
120,10,0,0q�6D   ��5      �� *	Cantitate+�� 5   
   $��Unit:��6   $��2��6    $��Center=�6-   $
��0,10q�6H   �� 5    
   -   5        $ unitTextBox��5       $��2��6'   $	��300��64   $��
120,10,0,0q�6@   ��5!      �� *UnitateDeMasura+�� 5#   
   $��Purchase Price:��6   $��3��6*   $��Center=�67   $
��0,10q�6R   �� 5$   
   -   5        $ purchasePriceTextBox��5$      $��3��60   $	��300��6=   $��
120,10,0,0q�6I   ��5%      �� *PretAchizitie+�� 5'   
   $��Expiration Date:��6   $��4��6+   $��Center=�68   $
��0,10q�6S   (  "System.Windows.Controls.DatePicker  5(   
   -   5          Name$ expirationDatePicker��5(      $��4��63   $	��300��6@   $��
120,10,0,0q�6L   	   SelectedDate	 5)      �� *DataExpirarii+�� 5+   
   $��	Add Stock��6   $	��100��6I   $��5��6U   $��
120,10,0,0q�6b   ��6%   �� *AddStockCommand+6b   5,      5-      
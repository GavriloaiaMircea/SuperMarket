   M S B A M L   `   `   `  ����F  BSupermarket, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Supermarket.ViewsSupermarket.Views  5   	   =#clr-namespace:Supermarket.ViewModelSupermarket.ViewModel  5   	   (    "Supermarket.Views.AddProductWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Supermarket.Views     5   	   2vm#clr-namespace:Supermarket.ViewModel     5   	     � Title$  AddProductWindow��5	   	   $	��450��6"   $	��800��6/   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Product Name:��6   $��0��6(   �� 5   
   $��0��6   $��0,5,0,0q�6   $	��300��6M   ��60   �� *ProductName+6M   �� 5   
   $��Barcode:��6   $��1��6#   �� 5   
   $��1��6   $��0,5,0,0q�6   $	��300��6I   ��60   �� *	Barcode+6I   �� 5   
   $��	Category:��6   $��2��6$   �� 5   
   $��2��6   $��0,5,0,0q�6     �� DisplayMemberPath$ NumeCategorie��5      $	��300��6_   ��5   1   �� *
Categories+ �� SelectedItem 5   5   �� *SelectedCategory+6_   �� 5   
   $��	Producer:��6   $��3��6$   �� 5   
   $��3��6   $��0,5,0,0q�6    $ NumeProducator��5       $	��300��6`   ��5   1   �� *	Producers+ 5    6   �� *SelectedProducer+6`   �� 5"   
   $��4��6   $��0,10,0,0q�6   $��Add Product��60   $	��300��6l   ��6F   �� *AddProductCommand+6l   5#      5$      
   M S B A M L   `   `   `  ����F  BSupermarket, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Supermarket.ViewsSupermarket.Views  5   	   ,    &Supermarket.Views.UserManagementWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Supermarket.Views     5   	     � Title$  UserManagementWindow��5   	   $	��450��6&   $	��800��63    � 	Resources 5
         UserTypesSource �� Source��%4   &	        �� 5   
    67   �� *	UserTypes+5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��10q�6.   .+�5      �� $��Add User��6   $��5q�6(   $	��100��63   ��6?   �� *AddUserCommand+�� 5      $��Save Changes��6   $��5q�6S   $	��100��6^   ��6,   �� *SaveChangesCommand+6^   �� 5      $��Delete User��6   $��5q�6+   $	��100��66   ��6B   �� *DeleteUserCommand+��6h   �� *SelectedUser+5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ userDataGrid��5         AutoGenerateColumns$ False��6G   $��1��6c   $��10q�6p      
IsReadOnly$ False��6|   ��6)   �� *Users+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 Username��6%   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 67   �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�  5      $	 Password��6%   
 67   �� *
Password+$ PropertyChanged=�  5      $	 	User Type��6%   
 68   �� *
UserType+$ PropertyChanged=�5       �� 5!   
   $ 
Horizontal=�6   $��10q�6.   $��2��6:   .+�5"      ��  �� DisplayMemberPath$ Username��65   $	��200��6x   $��5q�6�   ��6   �� *Users+ �� SelectedItem 6R   �� *SelectedUser+6�   (  "System.Windows.Controls.DatePicker  5#      $	��200��6@   $��5q�6L      SelectedDate 6   �� *SelectedMonth+6L   �� 5$      $��Show Incomes��6   $	��120��6S   $��5q�6_   ��6,   �� *ShowIncomesCommand+{�5%      �� *IsUserAndDateSelected+5&        5'   
   $ False��67   $��3��6S   $��10q�6`   ��6   �� *UserIncomes+ 5(        5)      $
	 Date��6%   
 63   �� *Date+  5*      $	 Income��6%   
 65   �� *Income+5,      5-      5.      
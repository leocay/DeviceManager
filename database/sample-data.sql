USE [DeviceManagerDb];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;
GO

SET IDENTITY_INSERT [dbo].[DeviceCategories] ON;
INSERT INTO [dbo].[DeviceCategories] ([CategoryId], [CategoryName], [Description]) VALUES
(1, 'Laptop', 'Portable work laptop'),
(2, 'Desktop', 'Office desktop PC'),
(3, 'Monitor', 'External display monitor'),
(4, 'Printer', 'Office laser printer'),
(5, 'Scanner', 'Document scanner'),
(6, 'Projector', 'Meeting room projector'),
(7, 'Network Switch', 'Managed network switch'),
(8, 'Router', 'Wireless router'),
(9, 'WiFi Access Point', 'Wireless access point'),
(10, 'UPS', 'Uninterruptible power supply'),
(11, 'Server', 'Rack server'),
(12, 'Keyboard', 'Wired keyboard'),
(13, 'Mouse', 'Wireless mouse'),
(14, 'Docking Station', 'Laptop docking station'),
(15, 'Tablet', 'Business tablet'),
(16, 'Smartphone', 'Company phone'),
(17, 'Headset', 'Noise cancelling headset'),
(18, 'Webcam', 'HD webcam'),
(19, 'SSD', 'Solid state drive'),
(20, 'HDD', 'Hard disk drive');
SET IDENTITY_INSERT [dbo].[DeviceCategories] OFF;
GO

SET IDENTITY_INSERT [dbo].[Employees] ON;
INSERT INTO [dbo].[Employees] ([EmployeeId], [EmployeeCode], [FullName], [Department], [Position], [Email], [PhoneNumber], [CreatedAt]) VALUES
(1, 'EMP001', 'Nguyen Van An', 'IT', 'Engineer', 'an.nguyen@company.local', '0901000001', '2025-01-05T08:30:00'),
(2, 'EMP002', 'Tran Thi Binh', 'Finance', 'Analyst', 'binh.tran@company.local', '0901000002', '2025-01-06T08:30:00'),
(3, 'EMP003', 'Le Quang Chi', 'HR', 'Specialist', 'chi.le@company.local', '0901000003', '2025-01-07T08:30:00'),
(4, 'EMP004', 'Pham Duc Dung', 'Sales', 'Staff', 'dung.pham@company.local', '0901000004', '2025-01-08T08:30:00'),
(5, 'EMP005', 'Hoang Mai Em', 'Marketing', 'Coordinator', 'em.hoang@company.local', '0901000005', '2025-01-09T08:30:00'),
(6, 'EMP006', 'Vu Thanh Giang', 'Operations', 'Manager', 'giang.vu@company.local', '0901000006', '2025-01-10T08:30:00'),
(7, 'EMP007', 'Do Minh Hai', 'IT', 'Lead', 'hai.do@company.local', '0901000007', '2025-01-11T08:30:00'),
(8, 'EMP008', 'Nguyen Thu Hanh', 'Procurement', 'Staff', 'hanh.nguyen@company.local', '0901000008', '2025-01-12T08:30:00'),
(9, 'EMP009', 'Bui Tuan Kiet', 'Logistics', 'Specialist', 'kiet.bui@company.local', '0901000009', '2025-01-13T08:30:00'),
(10, 'EMP010', 'Dao Lan Linh', 'Support', 'Staff', 'linh.dao@company.local', '0901000010', '2025-01-14T08:30:00'),
(11, 'EMP011', 'Ngo Quoc Minh', 'IT', 'Engineer', 'minh.ngo@company.local', '0901000011', '2025-01-15T08:30:00'),
(12, 'EMP012', 'Phan Thi Ngoc', 'Finance', 'Senior Analyst', 'ngoc.phan@company.local', '0901000012', '2025-01-16T08:30:00'),
(13, 'EMP013', 'Ly Van Oanh', 'HR', 'Manager', 'oanh.ly@company.local', '0901000013', '2025-01-17T08:30:00'),
(14, 'EMP014', 'Trinh Duc Phu', 'Sales', 'Specialist', 'phu.trinh@company.local', '0901000014', '2025-01-18T08:30:00'),
(15, 'EMP015', 'Huynh Gia Quoc', 'Operations', 'Supervisor', 'quoc.huynh@company.local', '0901000015', '2025-01-19T08:30:00'),
(16, 'EMP016', 'Ta My Tam', 'Marketing', 'Designer', 'tam.ta@company.local', '0901000016', '2025-01-20T08:30:00'),
(17, 'EMP017', 'Dang Hoai Uyen', 'Support', 'Technician', 'uyen.dang@company.local', '0901000017', '2025-01-21T08:30:00'),
(18, 'EMP018', 'Mai Van Vy', 'IT', 'Architect', 'vy.mai@company.local', '0901000018', '2025-01-22T08:30:00'),
(19, 'EMP019', 'Cao Thi Yen', 'Procurement', 'Analyst', 'yen.cao@company.local', '0901000019', '2025-01-23T08:30:00'),
(20, 'EMP020', 'Luong Duc Zin', 'Logistics', 'Coordinator', 'zin.luong@company.local', '0901000020', '2025-01-24T08:30:00');
SET IDENTITY_INSERT [dbo].[Employees] OFF;
GO

SET IDENTITY_INSERT [dbo].[Devices] ON;
INSERT INTO [dbo].[Devices] ([DeviceId], [DeviceCode], [DeviceName], [CategoryId], [EmployeeId], [Brand], [Model], [SerialNumber], [PurchaseDate], [WarrantyExpiryDate], [Status], [Quantity], [Location], [Note], [CreatedAt], [UpdatedAt]) VALUES
(1, 'DEV001', 'Laptop Dell Latitude 5440', 1, 1, 'Dell', 'Latitude 5440', 'DL5440-001', '2024-02-10T00:00:00', '2027-02-10T00:00:00', 'In Use', 1, 'IT Room A', 'Primary laptop for engineering work', '2025-02-01T09:00:00', '2025-03-01T10:00:00'),
(2, 'DEV002', 'Desktop HP ProDesk 400', 2, 2, 'HP', 'ProDesk 400', 'HP400-002', '2024-03-15T00:00:00', '2027-03-15T00:00:00', 'Available', 2, 'Storage Room 1', 'Spare desktop units', '2025-02-02T09:00:00', NULL),
(3, 'DEV003', 'Monitor Samsung 27', 3, 3, 'Samsung', 'S27R650', 'SS27-003', '2024-04-20T00:00:00', '2027-04-20T00:00:00', 'In Use', 1, 'HR Floor', 'Paired with office workstation', '2025-02-03T09:00:00', '2025-03-05T11:00:00'),
(4, 'DEV004', 'Printer Canon LBP', 4, 4, 'Canon', 'LBP6030', 'CNLBP-004', '2023-12-10T00:00:00', '2026-12-10T00:00:00', 'Maintenance', 1, 'Sales Floor', 'Paper feed issue reported', '2025-02-04T09:00:00', '2025-04-01T15:30:00'),
(5, 'DEV005', 'Scanner Epson DS', 5, 5, 'Epson', 'DS-530', 'EPDS-005', '2024-01-18T00:00:00', '2027-01-18T00:00:00', 'Available', 1, 'Archive Room', 'Ready for document digitization', '2025-02-05T09:00:00', NULL),
(6, 'DEV006', 'Projector BenQ MX', 6, NULL, 'BenQ', 'MX560', 'BQMX-006', '2024-05-11T00:00:00', '2027-05-11T00:00:00', 'Reserved', 1, 'Meeting Room 2', 'Booked for monthly review', '2025-02-06T09:00:00', '2025-03-20T13:00:00'),
(7, 'DEV007', 'Switch Cisco 24 Port', 7, 7, 'Cisco', 'SG250-24', 'CSC24-007', '2023-11-02T00:00:00', '2026-11-02T00:00:00', 'In Use', 1, 'Network Rack', 'Core access switch', '2025-02-07T09:00:00', '2025-03-12T10:15:00'),
(8, 'DEV008', 'Router TP-Link AX3000', 8, 8, 'TP-Link', 'AX3000', 'TPLA-008', '2024-06-09T00:00:00', '2027-06-09T00:00:00', 'Available', 1, 'IT Storage', 'Backup router unit', '2025-02-08T09:00:00', NULL),
(9, 'DEV009', 'Access Point Ubiquiti U6', 9, 9, 'Ubiquiti', 'UniFi U6', 'UBU6-009', '2024-07-01T00:00:00', '2027-07-01T00:00:00', 'In Use', 3, 'Office Ceiling', 'Covers open workspace area', '2025-02-09T09:00:00', '2025-03-18T12:00:00'),
(10, 'DEV010', 'UPS APC 1500VA', 10, NULL, 'APC', 'BR1500G', 'APC15-010', '2023-10-25T00:00:00', '2026-10-25T00:00:00', 'Available', 2, 'Power Room', 'Battery backup for critical desks', '2025-02-10T09:00:00', NULL),
(11, 'DEV011', 'Server Dell PowerEdge', 11, 11, 'Dell', 'PowerEdge R250', 'DPR250-011', '2024-08-15T00:00:00', '2027-08-15T00:00:00', 'Maintenance', 1, 'Server Room', 'OS patching scheduled', '2025-02-11T09:00:00', '2025-04-08T14:20:00'),
(12, 'DEV012', 'Keyboard Logitech K120', 12, 12, 'Logitech', 'K120', 'LGK120-012', '2024-02-28T00:00:00', '2027-02-28T00:00:00', 'In Use', 20, 'Office Cabinet', 'Bulk accessory stock', '2025-02-12T09:00:00', '2025-03-01T09:15:00'),
(13, 'DEV013', 'Mouse Logitech M185', 13, 13, 'Logitech', 'M185', 'LGM185-013', '2024-03-03T00:00:00', '2027-03-03T00:00:00', 'Available', 25, 'Office Cabinet', 'Spare wireless mouse units', '2025-02-13T09:00:00', NULL),
(14, 'DEV014', 'Docking Station Lenovo', 14, 14, 'Lenovo', 'ThinkPad Dock', 'LNDCK-014', '2024-05-19T00:00:00', '2027-05-19T00:00:00', 'In Use', 5, 'IT Room B', 'Assigned to hybrid workstations', '2025-02-14T09:00:00', '2025-03-07T16:45:00'),
(15, 'DEV015', 'Tablet Apple iPad 10', 15, 15, 'Apple', 'iPad 10th Gen', 'APIPD-015', '2024-09-21T00:00:00', '2027-09-21T00:00:00', 'Reserved', 4, 'Executive Office', 'Reserved for presentation use', '2025-02-15T09:00:00', '2025-03-25T10:10:00'),
(16, 'DEV016', 'Smartphone Samsung S24', 16, 16, 'Samsung', 'Galaxy S24', 'SSS24-016', '2024-10-05T00:00:00', '2026-10-05T00:00:00', 'In Use', 3, 'Mobile Cabinet', 'Issued for field coordination', '2025-02-16T09:00:00', '2025-03-14T09:50:00'),
(17, 'DEV017', 'Headset Jabra Evolve2', 17, 17, 'Jabra', 'Evolve2 40', 'JBEV2-017', '2024-04-12T00:00:00', '2027-04-12T00:00:00', 'Available', 10, 'Support Desk', 'Headsets for customer support', '2025-02-17T09:00:00', NULL),
(18, 'DEV018', 'Webcam Logitech C920', 18, 18, 'Logitech', 'C920', 'LGC920-018', '2024-11-11T00:00:00', '2027-11-11T00:00:00', 'Maintenance', 6, 'IT Storage', 'Firmware update pending', '2025-02-18T09:00:00', '2025-04-05T08:40:00'),
(19, 'DEV019', 'SSD Kingston 1TB', 19, 19, 'Kingston', 'NV2 1TB', 'KSNV2-019', '2024-12-02T00:00:00', '2027-12-02T00:00:00', 'In Use', 12, 'Spare Parts Room', 'Used for laptop upgrades', '2025-02-19T09:00:00', '2025-03-28T11:30:00'),
(20, 'DEV020', 'HDD Seagate 2TB', 20, 20, 'Seagate', 'Barracuda 2TB', 'STB2T-020', '2023-09-30T00:00:00', '2026-09-30T00:00:00', 'Retired', 8, 'Archive Storage', 'Replacement stock pending disposal', '2025-02-20T09:00:00', '2025-04-10T17:05:00');
SET IDENTITY_INSERT [dbo].[Devices] OFF;
GO

SET IDENTITY_INSERT [dbo].[DeviceLogs] ON;
INSERT INTO [dbo].[DeviceLogs] ([LogId], [DeviceId], [ActionType], [ActionBy], [ActionTime], [Content]) VALUES
(1, 1, 'Created', 'System', '2025-02-01T09:05:00', 'Device record was created and assigned to Nguyen Van An'),
(2, 2, 'Assigned', 'Tran Thi Binh', '2025-02-02T10:00:00', 'Desktop batch moved to storage waiting for deployment'),
(3, 3, 'Checked', 'Le Quang Chi', '2025-02-03T11:20:00', 'Monitor tested and marked ready for daily use'),
(4, 4, 'Maintenance', 'Pham Duc Dung', '2025-02-04T14:00:00', 'Printer reported paper feed issue and sent for service'),
(5, 5, 'Inventory', 'Hoang Mai Em', '2025-02-05T09:40:00', 'Scanner verified as available in archive room'),
(6, 6, 'Reserved', 'Vu Thanh Giang', '2025-02-06T13:15:00', 'Projector reserved for monthly review meeting'),
(7, 7, 'Updated', 'Do Minh Hai', '2025-02-07T15:10:00', 'Switch rack location updated after network cleanup'),
(8, 8, 'Checked', 'Nguyen Thu Hanh', '2025-02-08T10:30:00', 'Router backup unit inspected and passed basic test'),
(9, 9, 'Moved', 'Bui Tuan Kiet', '2025-02-09T16:25:00', 'Access point relocated to improve coverage'),
(10, 10, 'Available', 'Dao Lan Linh', '2025-02-10T11:45:00', 'UPS battery health confirmed after inspection'),
(11, 11, 'Maintenance', 'Ngo Quoc Minh', '2025-02-11T14:50:00', 'Server patch window started by infrastructure team'),
(12, 12, 'Issued', 'Phan Thi Ngoc', '2025-02-12T09:20:00', 'Keyboard stock issued to new office desk setup'),
(13, 13, 'Inventory', 'Ly Van Oanh', '2025-02-13T12:35:00', 'Mouse units counted and marked as spare stock'),
(14, 14, 'Assigned', 'Trinh Duc Phu', '2025-02-14T15:05:00', 'Docking stations assigned to hybrid employees'),
(15, 15, 'Reserved', 'Huynh Gia Quoc', '2025-02-15T10:10:00', 'Tablet reserved for upcoming management presentation'),
(16, 16, 'Issued', 'Ta My Tam', '2025-02-16T11:55:00', 'Smartphone assigned for field coordination tasks'),
(17, 17, 'Checked', 'Dang Hoai Uyen', '2025-02-17T13:25:00', 'Headset set tested for support desk usage'),
(18, 18, 'Maintenance', 'Mai Van Vy', '2025-02-18T14:40:00', 'Webcam firmware update scheduled by IT'),
(19, 19, 'Updated', 'Cao Thi Yen', '2025-02-19T16:15:00', 'SSD stock amount updated after replacement batch'),
(20, 20, 'Retired', 'Luong Duc Zin', '2025-02-20T17:30:00', 'HDD marked as retired and awaiting disposal process');
SET IDENTITY_INSERT [dbo].[DeviceLogs] OFF;
GO

COMMIT TRANSACTION;
GO
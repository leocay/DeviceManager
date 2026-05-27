USE [DeviceManagerDb];
GO

UPDATE [dbo].[DeviceCategories]
SET [Description] = CASE [CategoryId]
    WHEN 1 THEN '/images/device-categories/laptop.svg'
    WHEN 2 THEN '/images/device-categories/desktop.svg'
    WHEN 3 THEN '/images/device-categories/monitor.svg'
    WHEN 4 THEN '/images/device-categories/printer.svg'
    WHEN 5 THEN '/images/device-categories/scanner.svg'
    WHEN 6 THEN '/images/device-categories/projector.svg'
    WHEN 7 THEN '/images/device-categories/network-switch.svg'
    WHEN 8 THEN '/images/device-categories/router.svg'
    WHEN 9 THEN '/images/device-categories/wifi-access-point.svg'
    WHEN 10 THEN '/images/device-categories/ups.svg'
    WHEN 11 THEN '/images/device-categories/server.svg'
    WHEN 12 THEN '/images/device-categories/keyboard.svg'
    WHEN 13 THEN '/images/device-categories/mouse.svg'
    WHEN 14 THEN '/images/device-categories/docking-station.svg'
    WHEN 15 THEN '/images/device-categories/tablet.svg'
    WHEN 16 THEN '/images/device-categories/smartphone.svg'
    WHEN 17 THEN '/images/device-categories/headset.svg'
    WHEN 18 THEN '/images/device-categories/webcam.svg'
    WHEN 19 THEN '/images/device-categories/ssd.svg'
    WHEN 20 THEN '/images/device-categories/hdd.svg'
    ELSE [Description]
END
WHERE [CategoryId] BETWEEN 1 AND 20;
GO

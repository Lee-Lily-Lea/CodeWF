﻿CREATE TABLE `CmUser` (
    `Id`          varchar(50)   not null,
    `CreateBy`    varchar(50)   not null,
    `CreateTime`  datetime      not null,
    `ModifyBy`    varchar(50)   null,
    `ModifyTime`  datetime      null,
    `Version`     int           not null,
    `Extension`   text          null,
    `AppId`       varchar(50)   null,
    `CompNo`      varchar(50)   not null,
    `UserName`    varchar(50)   not null,
    `Password`    varchar(50)   not null,
    `OpenId`      varchar(50)   null,
    `UnionId`     varchar(50)   null,
    `NickName`    varchar(50)   null,
    `Sex`         varchar(50)   null,
    `Country`     varchar(50)   null,
    `Province`    varchar(50)   null,
    `City`        varchar(50)   null,
    `AvatarUrl`   varchar(250)  null,
    `Metier`      varchar(50)   null,
    `Status`      varchar(50)   not null,
    `Integral`    int           null,
    `ContentQty`  int           null,
    `ReplyQty`    int           null,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `CmCategory` (
    `Id`          varchar(50)   not null,
    `CreateBy`    varchar(50)   not null,
    `CreateTime`  datetime      not null,
    `ModifyBy`    varchar(50)   null,
    `ModifyTime`  datetime      null,
    `Version`     int           not null,
    `Extension`   text          null,
    `AppId`       varchar(50)   null,
    `CompNo`      varchar(50)   not null,
    `Type`        varchar(50)   not null,
    `ParentId`    varchar(50)   not null,
    `Code`        varchar(50)   not null,
    `Name`        varchar(50)   not null,
    `Icon`        varchar(50)   null,
    `Sort`        int           not null,
    `Enabled`     varchar(50)   not null,
    `Note`        text          null,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `CmPost` (
    `Id`          varchar(50)   not null,
    `CreateBy`    varchar(50)   not null,
    `CreateTime`  datetime      not null,
    `ModifyBy`    varchar(50)   null,
    `ModifyTime`  datetime      null,
    `Version`     int           not null,
    `Extension`   text          null,
    `AppId`       varchar(50)   null,
    `CompNo`      varchar(50)   not null,
    `Type`        varchar(50)   not null,
    `UserId`      varchar(50)   not null,
    `CategoryId`  varchar(50)   null,
    `Title`       varchar(250)  not null,
    `Content`     text          not null,
    `Summary`     varchar(500)  null,
    `Tags`        varchar(200)  null,
    `Image`       varchar(250)  null,
    `Files`       varchar(250)  null,
    `Status`      varchar(50)   not null,
    `PublishTime` datetime      null,
    `ViewQty`     int           null,
    `LikeQty`     int           null,
    `ReplyQty`    int           null,
    `RankNo`      int           null,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `CmReply` (
    `Id`          varchar(50)   not null,
    `CreateBy`    varchar(50)   not null,
    `CreateTime`  datetime      not null,
    `ModifyBy`    varchar(50)   null,
    `ModifyTime`  datetime      null,
    `Version`     int           not null,
    `Extension`   text          null,
    `AppId`       varchar(50)   null,
    `CompNo`      varchar(50)   not null,
    `BizType`     varchar(50)   not null,
    `BizId`       varchar(50)   not null,
    `UserId`      varchar(50)   not null,
    `Content`     text          not null,
    `ReplyTime`   datetime      not null,
    `LikeQty`     int           null,
    `ReplyQty`    int           null,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `CmLog` (
    `Id`          varchar(50)   not null,
    `CreateBy`    varchar(50)   not null,
    `CreateTime`  datetime      not null,
    `ModifyBy`    varchar(50)   null,
    `ModifyTime`  datetime      null,
    `Version`     int           not null,
    `Extension`   text          null,
    `AppId`       varchar(50)   null,
    `CompNo`      varchar(50)   not null,
    `BizType`     varchar(50)   not null,
    `LogType`     varchar(50)   not null,
    `BizId`       varchar(50)   not null,
    `UserId`      varchar(50)   not null,
    `UserIP`      varchar(50)   null,
    PRIMARY KEY (`Id`)
);
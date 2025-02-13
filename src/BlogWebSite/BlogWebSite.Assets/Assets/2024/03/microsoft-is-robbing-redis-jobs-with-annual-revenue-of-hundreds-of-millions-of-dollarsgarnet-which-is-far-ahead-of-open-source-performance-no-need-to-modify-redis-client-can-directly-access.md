---
title: 微软开抢年收入上亿美元的 Redis 饭碗？开源性能遥遥领先的 Garnet：无需修改，Redis 客户端可直接接入
slug: microsoft-is-robbing-redis-jobs-with-annual-revenue-of-hundreds-of-millions-of-dollarsgarnet-which-is-far-ahead-of-open-source-performance-no-need-to-modify-redis-client-can-directly-access
description: 近日，微软正式开源缓存存储系统 Garnet。据微软研究院数据库小组高级首席研究员 Badrish Chandramouli 介绍，Garnet 项目是从零开始构建而成，且以性能为核心考量（特别是吞吐量中的线程可扩展性与更高比例的低延迟水平）。
date: 2024-03-20 23:07:51
lastmod: 2024-03-21 00:26:47
copyright: Reprinted
banner: false
author: 凌敏,核子可乐
originalTitle: 微软开抢年收入上亿美元的 Redis 饭碗？开源性能遥遥领先的 Garnet：无需修改，Redis 客户端可直接接入
originalLink: https://www.infoq.cn/article/ppO8VZPtH59mqj8Np0HN
draft: false
cover: https://img1.dotnet9.com/2024/03/cover_09.png
categories: 
    - .NET
tags: 
    - Redis
    - Garnet
---

![微软开抢年收入上亿美元的 Redis 饭碗？开源性能遥遥领先的 Garnet：无需修改，Redis 客户端可直接接入](https://img1.dotnet9.com/2024/03/cover_09.png)

> Redis 和 Dragonfly 该有危机感了！

## 微软开源全新缓存存储系统 Garnet

近日，微软正式开源缓存存储系统 Garnet。据微软研究院数据库小组高级首席研究员 Badrish Chandramouli 介绍，Garnet 项目是从零开始构建而成，且以性能为核心考量（特别是吞吐量中的线程可扩展性与更高比例的低延迟水平）。

具体来说，Garnet 具有以下几大优势：

- Garnet 采用流行的 RESP 线路协议作为起点，因此大多数用户可以不作任何修改、就直接通过大多数编程语言编写的 Redis 客户端直接接入 Garnet。
- Garnet 通过多条客户端连接与小批量形式提供更好的可扩展性与吞吐量，帮助大型应用程序和服务节约运行成本。
- Garnet 在第 99 及第 99.9 百分位上表现出更好的客户端延迟水平，更高比例的稳定性表现对于现实场景而言至关重要。
- Garnet 基于最新.NET 技术，具有跨平台、可扩展和现代化等特点。它在设计上易于开发与调整，且不致牺牲常见场景下的性能水平。通过利用.NET 丰富的库生态来扩展其 API，并提供开放的优化机会。凭借对.NET 的充分发掘，Garnet 在 Linux 和 Windows 平台上均表现出顶尖性能。

据了解，微软研究院自 2016 年以来一直在研究现代键-值数据库架构。2018 年，微软将一套嵌入式键-值库 FASTER 开源之后，其性能超出原有系统几个数量级，同时专注于简单的单节点进程内键-值模型。

从 2021 年开始，根据实际用例的需求，微软开始构建一套新的远程缓存存储方案。其中包含一切必要功能，以作为现有缓存存储的可行替代选项。当时微软面临的挑战包括保持/增强其在早期工作中已经取得的性能优势，同时考虑如何更好地适应更加现实的普遍网络环境。这项工作的成果就是 Garnet。

在被问及 Garnet 适合部署在哪些场景下时，Chandramouli 表示任何“**使用 Redis、KeyDB 或者 Dragonfly 作为缓存存储方案的现有应用程序都适合**，Garnet 能提供更高的吞吐量、更低延迟、通过减少需要托管的缓存存储分片来降低成本，还可将数据溢出至本地磁盘或 SSD 以缓存超过内存大小的数据。此外，Garnet 也适合各种希望借极高性能缓存层提高性能、降低后端存储服务器或数据库成本的新型应用程序。”

![img](https://img1.dotnet9.com/2024/03/0901.png)

API 功能方面，Garnet 支持广泛的 API，包括原始字符串、分析与对象操作。它还提供分片、复制及动态密钥迁移等功能的集群模式。Gartner 支持客户端 RESP 事务及用 C#编写的服务器端存储过程，还允许用户在原始字符串及新对象类型之上设置自定义操作。所有这些均可简单使用 C#编写，因此自定义扩展的开发门槛更低。

网络、存储、集群功能方面，Garnet 使用快速且可插拔的网络层，且支持后续扩展，例如配合内核旁路堆栈。它支持传输层安全（TLS）通信协议和各种基本访问控制。Garnet 的存储层被称为 Tsavorite，是从 OSS FASTER 中分叉而成，可提供一系列强大的数据库功能，例如线程可扩展性、分层存储支持（内存、SSD 和云存储等）、快速非阻塞检查点、恢复、持久操作日志记录、多键事务支持，以及更好的内在管理与重用功能等。此外，Garnet 还支持集群操作模式。

除了单节点执行之外，Garnet 还支持集群模式，允许用户创建并管理分片和复制部署。Garnet 还支持高效、动态的键迁移方案，借此重新均衡各个分片。用户可以使用标准的 Redis 集群命令来创建并管理 Garnet 集群，各节点则执行 gossip 以共享并演进集群状态。总的来说，Garnet 的集群模式是一项庞大且仍在发展的功能，微软表示，更多细节将在后续文章中与大家分享。

Chandramouli 在回复 The Stack 的邮件中补充道，“我们也期待大家能将 Garnet 在各类其他现实应用中的表现反馈回来。此外，我们还拥有一套基于 C#的强大存储过程模型，用户可以借此对关注的事务进行自定义。最后，我们将 Garnet 视为面向未来的重要创新工具，包括优化磁盘 IO、内核旁路网络以及向量数据库等应用场景。”

## Garnet 有什么亮点？

云和边缘计算的快速增长让相关应用程序和服务在数据和覆盖范围上均有显著提升。但与此同时，它们也在数据访问、更新与转换层面提出了效率更高、延迟更低、成本更廉的实际要求。这些应用程序与服务往往需要在存储交互方面投入大量运营支出，这也使其成为当今最昂贵、最具挑战性的平台领域之一。以单独可扩展的远程进程形式存在的缓存存储软件层，能够有效降低这些成本并提高应用程序性能。这也推动了缓存存储行业的发展，包括许多大家耳熟能详的开源系统，例如 Redis、Memcached、KeyDB 以及 Dragonfly。

与仅支持简单获取/设置接口的传统远程缓存存储不同，现代缓存需要提供丰富的 API 与功能集。它们支持原始字符串、Hyperloglog 等分析数据结构，以及排序集和哈希等复杂数据类型。它们还须允许用户为缓存设置检查点和恢复功能、创建数据分片、维护复制副本并支持事务与自定义扩展。

然而，现有系统在保持系统设计简单性的同时，往往难以满足如此丰富的功能需求，包括导致其无法充分利用最新硬件功能（例如多核心、分层存储、快速网络）。此外，其中许多系统在设计之初，也没有考虑到可由应用程序开发者轻松扩展、或者在不同平台/操作系统上良好运行等现实需求。

根据介绍，Garnet 在设计上重新考量了整个缓存存储堆栈——从网络处获取数据包、到解析和处理数据库操作、再到执行存储交互。

下图为 Garnet 的整体架构，可以看到，Garnet 的网络层继承了微软受 ShadowFax 研究启发所建立的共享内存设计。TLS 处理与存储交互在 IO 完成线程上执行，这就避免了常见的线程切换开销。这种方法能够借 CPU 缓存一致性将数据传输至网络，而非基于需要在服务器上移动数据的传统 shuffle 设计。

![Garnet项目整体架构](https://img1.dotnet9.com/2024/03/0902.png)

Garnet 的存储设计由两套 Tsavorite 键-值存储组成，二者与统一的操作日志进行绑定。前一套存储被称为“主存储”，针对原始字符串操作进行了优化，负责管理内存以避免垃圾收集。第二套则为可选的“对象存储”，主要针对复杂对象及自定义数据类型进行优化，具体涵盖排序集、集、哈希、列表和地理空间等流行数据类型。它们被存储在内存堆上（以保证更新更加高效），并以序列化形式存放在磁盘内。未来，微软还将研究如何通过统一的索引与日志简化 Garnet 的系统维护。

Garnet 设计中的一大显著特点，就是采用了 Tsavorite 存储 API。该 AIP 用于提供更大、更丰富且可扩展的 RESP API 表面，能够执行读取、更新插入、删除以及原子读取-修改-写入等操作，且全部通过 Garnet 的异步回调实现以便在每项操作期间的多个点上插入逻辑。存储 API 模型还确保 Garnet 能够将对问题的解析与查询处理，同并发、存储分层和检查点等其他存储功能彻底分开。

此外，Garnet 还进一步增加了对基于双阶段锁定的多键事务的支持。用户可以使用 RESP 客户端事务（MULTI-EXEC）或使用 C#中的服务器端事务存储过程。

### 性能表现

微软研究团队通过展示比较了 Gartner 与其他领先开源缓存存储方案间的关键性能指标。

首先，团队预先配置了两套运行 Linux 系统（Ubuntu 20.04）的 Azure 标准 F72s v2 虚拟机（每虚拟机 72 上 vCPU 加 144 GiB 内存），且启用了加速 TCP。其中一套虚拟机运行各种缓存存储服务器，另一套则专门发布工作负载。这里微软使用自己的基准测试工具 Resp.benchmark，统一由它给出性能测试结果。

微软将 Garnet 与最新开源版本的 Redis（v7.2）、KeyDB（v6.3.4）以及 Dragonfly（v6.2.11）进行了比较。在实验中，微软使用了均匀随机分布的键（Garnet 的共享内存设计对于非随机分布的键具有更好的性能优化效果）。在这些实验中，数据会被预先加载至每台服务器上，再嵌入内存中。

#### 实验一：不同数量客户端会话的吞吐量比较

从大指 GET 操作（每批 4096 条请求）加低负载（8 字节键与值）起步，尝试最大限度减少网络开销，并逐步增加客户端会话数量以比较系统性能。从下图中可以看到，Garnet 表现出的可扩展性超越了 Redis 与 KeyDB，同时实现了比所有三大基线系统更高的吞吐量（y 轴取对数坐标）。请注意，虽然 Dragonfly 的扩展性能与 Garnet 类似，但前者属于纯内存内系统。此外，当数据库大小（即预加载的键数量）明显超过处理器的缓存大小时（2.56 亿个键），Garnet 相较于其他系统仍拥有强劲的吞吐量表现。

![img](https://img1.dotnet9.com/2024/03/0903.png)

数据库大小为（a）1024 个键及（b）2.56 亿个键时，不同数量客户端会话对应的吞吐量（对数坐标）。

#### 实验二：不同批量大小的吞吐量比较

接下来，使用 GET 操作加固定数量（64）的客户端会话来改变批量大小。跟之前的实验一样，继续尝试两种不同的数据库大小。如下图所示，即使不采用分批处理，Garnet 的性能同样表现更好；而在采用分批处理后，即使批量规模很小，Garnet 的性能优势也在增强。负载大小与实验一相同，且 y 轴同样取对数坐标。

![img](https://img1.dotnet9.com/2024/03/0904.png)

数据库大小为（a）1024 个键及（b）2.56 亿个键时，不同批量大小下的吞吐量比较（取对数坐标）。

#### 实验三：不同数量实施意见会话的延迟比较

接下来测试的是各种系统的客户端延迟。如下图所示，随着客户端会话数量增加，与其他系统相比，Garnet 在各个百分位上的延迟（以微秒为单位）均更低也更加稳定。实验中，以 GET 操作占 80%、SET 操作占 20%的混合比例发送操作，且不做分批处理。

![img](https://img1.dotnet9.com/2024/03/0905.png)

不同客户端会话数量时，（a）中位数、（b）第 99 百分位与（c）第 99.9 百分位处的延迟水平。

#### 实验四：不同批量大小的延迟比较

Garnet 的延迟水平针对自适应客户端的批量与查询系统进行了优化。微软将批量大小从 1 增加到 64，并在下图中整理出具有 128 个活动客户端连接时不同百分位上的延迟水平。从下图中可以看到，Gartner 的延迟整体较低。与之前的实验一样，同样采用 GET 操作占 80%、SET 操作占 20%的混合比例。

![img](https://img1.dotnet9.com/2024/03/0906.png)

不同批量大小下，（a）中位数、（b）第 99 百分位以及（c）第 99.9 百分位上的延迟水平。

## 开发者：Redis 需要进行重大性能优化了！

从基准性能图表来看，GET 命令的吞吐量超过了 Dragonfly 十倍以上。虽然第 50 百分位的延迟水平略高于 Dragonfly，但第 99 百分位上的延迟却比 Dragonfly 更低。Garnet 和 Dragonfly 在吞吐量和延迟上的表现均远远优于 Redis，不少开发者认为，这表明 Redis 可能需要进行重大性能优化。

开发者 hipadev23 表示，“Garnet 确实是首个在低并发与高并发水平上均优于 Redis 的替代方案，这是一项很了不起的成就。”“Redis 可能需要进行重大性能优化。”

开发者 mtmk 认为，对于需要直接在微软 Windows Server 上运行 Redis（或者兼容），但又不想依赖于 WSL2 的朋友们来说，Garnet 的出现肯定是个好消息。以往由 Redis 端口（现处于归档状态）造成的内存使用问题（主要是由于内存映射文件 AFAIK）将不复存在。

也有不少开发者仍旧坚定地选择 Redis。Redis 在某些方面对开发者更友好，而且运行时间更长更稳定。对于 Garnet，大家在许可协议、产品定价、更新维护等方面普遍较为担心。throwaway38375 表示，“Redis 在许可协议或者产品定价方面应该会更稳定，而且它毕竟经历了数十亿小时的生产运行考验。Redis 也更容易安装和理解。”Someone 认为，“对于这样一个微软研究院推出的项目，我最担心的不是许可协议和产品定价，而是缺乏更新（功能、维护甚至是安全更新）”。

### By the way：Garnet 是用 C#开发的

在社区讨论中，不少开发者惊讶于 Garnet 项目居然是用 C#开发的。

开发者 west0n 表示：“最让我惊讶的是，Garnet 项目居然是用 C#开发的，而 Dragonfly 是用 C++开发的，Redis 则是用 C 开发的。”开发者 whimsicalism 更是直言“太意外了，垃圾收集语言 C#编写的 Garnet 居然击败了 Redis 和 Dragonfly。”

也有开发者对此给出的评价较为中肯，pjmlp 认为“垃圾收集语言跟垃圾收集语言可不一样，像 C#和.NET 这些语言其实提供了跟 C++相当的所有性能调优选项。”他表示，大家该做的是认真学习，而不是把所有垃圾收集语言都归为一类，再一棒子打死。【**站长注：.NET 是一个平台，C#是.NET 的一个实现，C#与.NET 类比 Java 与 JDK**】

此外，更具体地讲，MSIL 和.NET 在设计上也能支持 C++，而 C#和 F#等语言也有办法访问这些功能。即使某些功能未在语言语法层面公开，开发者也可以直接使用 C++/CLI 生成的 MSIL。

对此，你怎么看呢？欢迎在评论区留下你的观点。

参考链接：

https://www.microsoft.com/en-us/research/blog/introducing-garnet-an-open-source-next-generation-faster-cache-store-for-accelerating-applications-and-services/

https://www.thestack.technology/microsoft-takes-on-redis-with-new-open-source-garnet-cache-store/

https://news.ycombinator.com/item?id=39752504

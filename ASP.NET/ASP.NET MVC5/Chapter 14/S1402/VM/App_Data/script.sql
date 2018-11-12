--商品表（Product）
CREATE TABLE [Product](
	[ProductId]		[VARCHAR](50)		NOT NULL, --ID
	[Name]			[NVARCHAR](256)		NOT NULL, --片名
	[Genre]			[NVARCHAR](50)		NOT NULL, --影片类型
	[Starring]		[NVARCHAR](50)		NOT NULL, --领衔主演
	[SupportingActors]	[VARCHAR](256)		NOT NULL, --主演
	[Director]		[NVARCHAR](50)		NOT NULL, --导演
	[ScriptWriter]		[NVARCHAR](50)		NOT NULL, --编剧
	[ProductionCountry] 	[NVARCHAR](50)		NOT NULL, --制片国家
	[ProductionCompany]	[NVARCHAR](50)		NOT NULL, --制片公司
	[ReleaseYear]		[INT]			NOT NULL, --发行年份
	[Language]		[NVARCHAR](50)		NOT NULL, --对白语言
	[RunTime]		[INT]			NOT NULL, --片长（分钟）
	[Price]			[DECIMAL](18, 0)	NOT NULL, --单价	
	[Poster]		[VARCHAR](50)		NOT NULL, --电影海报图片名称
	[Stock]			[INT]			NOT NULL, --库存
	[Story]			[NVARCHAR](max)		NOT NULL, --剧情介绍（完整）
	[StoryAbstract]		[NVARCHAR](1000)	NOT NULL, --剧情介绍（摘要）
CONSTRAINT [C_Product_PK]     
    PRIMARY KEY CLUSTERED    ( [ProductId] ASC ) 
ON [PRIMARY]) ON [PRIMARY]

--订单表（Order）:
CREATE TABLE [Order](
	[OrderId]	[VARCHAR](50)		NOT NULL, --订单ID
	[UserName]	[VARCHAR](50)		NOT NULL, --客户账号
	[OrderTime] 	[DATETIME]		NOT NULL, --订单提交时间
 CONSTRAINT [C_Order_PK]     
            PRIMARY KEY CLUSTERED    ( [OrderId] ASC ) 
            ON [PRIMARY]) ON [PRIMARY]

--订单明细表（OrderLine）
CREATE TABLE [OrderLine](
	[OrderId]	[VARCHAR](50)		NOT NULL, --订单ID
	[ProductId] 	[VARCHAR](50)		NOT NULL, --商品ID
	[Quantity]	[INT]			NOT NULL, --订购数量
CONSTRAINT [C_OrderLine_PK]       
	PRIMARY KEY CLUSTERED ( [ProductId]  ASC,[OrderId] ASC ) 
	ON [PRIMARY]) ON [PRIMARY]

ALTER TABLE [OrderLine]  WITH CHECK ADD  CONSTRAINT [FK_OrderLine_Order]   
    FOREIGN KEY([OrderId])
REFERENCES [Order] ([OrderId])

ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD  CONSTRAINT [FK_OrderLine_Product] 
    FOREIGN KEY([ProductId])
    REFERENCES [dbo].[Product] ([ProductId])
GO

--添加应用记录    
INSERT INTO [dbo].[aspnet_Applications]
           ([ApplicationName]
           ,[LoweredApplicationName]
           ,[ApplicationId]
           ,[Description])
 VALUES
           ('VM'
           ,'vm'
           ,'85562079-171F-4E80-8E8E-929221720F78'
           ,'Video Mall')


--001：威尼斯商人（The Merchant of Venice）
INSERT INTO [dbo].[Product]
           ([ProductId]
           ,[Name]
           ,[Genre]
           ,[Starring]
           ,[SupportingActors]
           ,[Director]
           ,[ScriptWriter]
           ,[ProductionCountry]
           ,[ProductionCompany]
           ,[ReleaseYear]
           ,[Language]
           ,[RunTime]
           ,[Price]
           ,[Poster]
           ,[Stock]
           ,[Story]
           ,[StoryAbstract])
VALUES	  ('001' 
           ,'威尼斯商人（The Merchant of Venice）'
           ,'剧情'
           ,'阿尔·帕西诺|约瑟夫·费因斯'
           ,'杰瑞米·艾恩斯|琳恩·柯林斯|祖雷克哈·罗宾逊|克里斯·马歇尔|查理·考克斯'
           ,'迈克尔·莱德福'
           ,'迈克尔·莱德福 / 威廉·莎士比亚'
           ,'美国 / 意大利 / 卢森堡 / 英国'
           ,'米高梅影业公司'
           ,2004
           ,'英语'
           ,134
           ,45
           ,'威尼斯商人.jpg'
           ,10
           ,'年轻人巴萨尼奥（约瑟夫·费因斯 Joseph Fiennes饰）一心要向富商千金鲍西娅小姐（琳恩·柯林斯 Lynn Collins饰）求婚，为了这个愿望，他必须付出三千达克特，无奈中他只好向热心肠的商人安东尼奥（杰里米·艾恩斯 Jeremy Irons饰）借钱。碰巧的是，安东尼奥因为生意周转的问题，只能问犹太人夏洛克（阿尔·帕西诺 Al Pacino饰）借来这笔钱。<p>安东尼奥对犹太人历来抱有强烈的厌恶感，而夏洛克则提出了让人吃惊的要求：安东尼奥必须如期如数的归还这笔钱，否则，他将会切下安东尼奥身上的一磅肉。</p><p>巴萨尼奥终于如愿以偿的娶回了鲍西娅，安东尼奥却因为生意失败，在夏洛克的讨债中陷入困境。夏洛克还要趁这个机会，一雪安东尼奥对犹太人侮辱之耻。安东尼奥那一磅肉，看来非割不可了，聪明的鲍西娅却用绝妙一招，化险为夷。</P>'
           ,'年轻人巴萨尼奥（约瑟夫·费因斯 Joseph Fiennes饰）一心要向富商千金鲍西娅小姐（琳恩·柯林斯 Lynn Collins饰）求婚，为了这个愿望，他必须付出三千达克特，无奈中他只好向热心肠的商人安东尼奥（杰里米·艾恩斯 Jeremy Irons饰）借钱。碰巧的是，安东尼奥因为生意周转的问题，只能问犹太人夏洛克（阿尔·帕西诺 Al Pacino饰）借来这笔钱...')

--002：女人香（Scent of a Woman）
INSERT INTO [dbo].[Product]
           ([ProductId]
           ,[Name]
           ,[Genre]
           ,[Starring]
           ,[SupportingActors]
           ,[Director]
           ,[ScriptWriter]
           ,[ProductionCountry]
           ,[ProductionCompany]
           ,[ReleaseYear]
           ,[Language]
           ,[RunTime]
           ,[Price]
           ,[Poster]
           ,[Stock]
           ,[Story]
           ,[StoryAbstract])
VALUES	  ('002' 
           ,'女人香（Scent of a Woman）'
           ,'剧情'
           ,'阿尔·帕西诺|克里斯·奥唐纳'
           ,'詹姆斯·瑞布霍恩|加布里埃尔·安瓦尔|菲利普·塞默·霍夫曼'
           ,'马丁·布莱斯'
           ,'博·古德曼 / Giovanni Arpino'
           ,'美国'
           ,'环球影业公司'
           ,1992
           ,'英语'
           ,157
           ,45
           ,'女人香.jpg'
           ,10
           ,'查理（克里斯?奥唐纳饰）是一个普通的中学生，他因为见证着一件恶作剧但又不想出卖朋友，于是面临着一道艰难的选择题——要么坦白，要么被学校勒退。而史法兰（阿尔?帕西诺 Al Pacino 饰）是一个退伍军官，因为意外失明，人生从顶点的辉煌滑落谷底。<p>自从两人的人生有了交集，命运的转折慢慢清晰。史法兰打算结束失去了意义的生活，于是在查理的陪伴下，尽情享受着人生最后一趟出行。他吃最美味的佳肴，买光鲜的汽车，在狭窄的街道上飚车，住在豪华的套房里，跟随陌生女子的袅袅香水味，跳一段优雅性感的探戈……极乐的疯狂后，他拿枪对准了自己的头颅。然而，查理声嘶力竭的劝阻改变了一切，曙光亦渐渐在这个老人心中重新升起，二人情深如父子。</p><p>史法兰在学校礼堂激昂演说，挽救了查理的前途，讽刺了学校的伪善。二人在互相鼓舞中得到重生。</p>'
           ,'查理（克里斯?奥唐纳饰）是一个普通的中学生，他因为见证着一件恶作剧但又不想出卖朋友，于是面临着一道艰难的选择题——要么坦白，要么被学校勒退。而史法兰（阿尔?帕西诺 Al Pacino 饰）是一个退伍军官，因为意外失明，人生从顶点的辉煌滑落谷底...')

--003：忠奸人（Donnie Brasco）
INSERT INTO [dbo].[Product]
           ([ProductId]
           ,[Name]
           ,[Genre]
           ,[Starring]
           ,[SupportingActors]
           ,[Director]
           ,[ScriptWriter]
           ,[ProductionCountry]
           ,[ProductionCompany]
           ,[ReleaseYear]
           ,[Language]
           ,[RunTime]
           ,[Price]
           ,[Poster]
           ,[Stock]
           ,[Story]
           ,[StoryAbstract])
VALUES	  ('003' 
           ,'忠奸人（Donnie Brasco）'
           ,'剧情|惊悚|犯罪'
           ,'阿尔·帕西诺|约翰尼·德普'
           ,'迈克尔·马德森|布鲁诺·柯比|詹姆斯·拉索|安·海切|保罗·吉亚玛提'
           ,'迈克·内威尔'
           ,'保罗·阿塔那斯奥'
           ,'美国'
           ,'康斯坦丁影业公司'
           ,1997
           ,'英语'
           ,127
           ,45
           ,'忠奸人.jpg'
           ,10
           ,'为了对付黑社会，联邦调查局的探员皮斯顿（约翰尼?戴普 饰）不得不告别妻子和三个女儿，化名唐尼?布拉斯科打入黑帮鲁吉罗家族卧底。为了取得帮派内部成员的信任，唐尼在头目“老左”（阿尔?帕西诺 饰）的带领下，参加了一系列的犯罪活动，也逐渐掌握了他们的犯罪证据。<p>然而，长时间的相处中，“老左”对唐尼信任有加，唐尼也见识到“老左”温情的一面，慢慢地对他产生了敬佩之情，傍徨不定，不再向上级及时汇报情况；同时，由于卧底工作的特殊性，唐尼极少和妻女见面，家庭生活濒临崩溃。</p><p>此时，帮派内部的仇杀开始了，“老左”的上级黑桑铲除异己，掀起了一场腥风血雨。唐尼也将面对或“忠”或“奸”的抉择。</p>'
           ,'为了对付黑社会，联邦调查局的探员皮斯顿（约翰尼?戴普 饰）不得不告别妻子和三个女儿，化名唐尼?布拉斯科打入黑帮鲁吉罗家族卧底。为了取得帮派内部成员的信任，唐尼在头目“老左”（阿尔?帕西诺 饰）的带领下，参加了一系列的犯罪活动，也逐渐掌握了他们的犯罪证据...')
--004：盗火线（Heat） 
INSERT INTO [dbo].[Product]
           ([ProductId]
           ,[Name]
           ,[Genre]
           ,[Starring]
           ,[SupportingActors]
           ,[Director]
           ,[ScriptWriter]
           ,[ProductionCountry]
           ,[ProductionCompany]
           ,[ReleaseYear]
           ,[Language]
           ,[RunTime]
           ,[Price]
           ,[Poster]
           ,[Stock]
           ,[Story]
           ,[StoryAbstract])
VALUES	  ('004' 
           ,'盗火线（Heat）'
           ,'剧情|惊悚|犯罪'
           ,'阿尔·帕西诺|罗伯特·德尼罗'
           ,'方·基默|强·沃特|汤姆·塞兹摩尔|艾米·布伦尼曼|艾什莉·贾德|娜塔丽·波特曼|丹尼·特乔'
           ,'迈克尔·曼'
           ,'迈克尔·曼'
           ,'美国'
           ,'华纳兄弟影业公司'
           ,1995
           ,'英语 / 西班牙语'
           ,170
           ,45
           ,'盗火线.jpg'
           ,10
           ,'文森特?汉纳（阿尔?帕西诺 饰）是洛杉矶警局重案组的探长，工作积极投入，打击犯罪毫不手软，因此，家庭生活危机重重，目前的第三次婚姻也面临解体；尼尔?麦考利（罗伯特?德尼罗 饰）是职业匪徒，混迹江湖多年，做事干净利落，而且心狠手辣，不留后患，组织手下成功抢劫运钞车的案子，使他受到了警方的“特别关注”。<p>一个偶然的机会，尼尔认识了平面设计师伊迪，两人一见钟情，再看到手下都有安定的家庭生活，尼尔也想把握自己的幸福，遂萌生退意。然而，在最后一次抢劫银行的行动中，因为叛徒的出卖，手下一死一伤，本已逃出的尼尔，带着伊迪在去机场的路上，得知了叛徒的消息，他决定替手下报仇，同时也和文森特?汉纳进行最后的较量...</p>'
           ,'文森特?汉纳（阿尔?帕西诺 饰）是洛杉矶警局重案组的探长，工作积极投入，打击犯罪毫不手软，因此，家庭生活危机重重，目前的第三次婚姻也面临解体；尼尔?麦考利（罗伯特?德尼罗 饰）是职业匪徒，混迹江湖多年，做事干净利落，而且心狠手辣，不留后患，组织手下成功抢劫运钞车的案子，使他受到了警方的“特别关注”...')
--005：魔鬼代言人（The Devil's Advocate）
INSERT INTO [dbo].[Product]
           ([ProductId]
           ,[Name]
           ,[Genre]
           ,[Starring]
           ,[SupportingActors]
           ,[Director]
           ,[ScriptWriter]
           ,[ProductionCountry]
           ,[ProductionCompany]
           ,[ReleaseYear]
           ,[Language]
           ,[RunTime]
           ,[Price]
           ,[Poster]
           ,[Stock]
           ,[Story]
           ,[StoryAbstract])
VALUES	  ('005' 
           ,'魔鬼代言人（The Devil''s Advocate）'
           ,'剧情|惊悚|犯罪'
           ,'基努·李维斯|阿尔·帕西诺'
           ,'查理兹·塞隆|杰弗瑞·琼斯|康妮·尼尔森'
           ,'泰勒·海克福德'
           ,'Jonathan Lemkin / 托尼·吉尔罗伊'
           ,'美国 / 德国'
           ,'华纳兄弟影业公司'
           ,1997
           ,'英语 / 德语 / 意大利语 / 汉语普通话 / 西班牙语'
           ,144
           ,45
           ,'魔鬼代言人.jpg'
           ,10
           ,'凯文（Keanu Reeves饰）是个保持不败记录的年轻律师，娇妻相伴，事业有为的他宛如生活在天堂一般。受到纽约著名事务所的邀请，他携妻子来到纽约。奢华的生活近在眼前，一切似乎都昭示着明天的坦途与光明。<p>老板米尔顿（Al Pacino饰）对凯文非常赏识，不断地委以重任，同时也带他见识了五光十色的世界。面临着工作的压力和色情，名利各个方面的诱惑，凯文渐渐的迷失在生活中，甚至忽视了妻子精神上的反常。直到失常的妻子自刎在他的臂弯中，凯文才猛然意识到身边的危机。</p><p>凯文愤怒的指责米尔顿给他设下陷阱。然而，究竟是魔鬼的诱惑还是内心的虚荣将凯文引向今日的迷雾与深渊？自由意志下凯文又会做出怎样的抉择？</p>'
           ,'凯文（Keanu Reeves饰）是个保持不败记录的年轻律师，娇妻相伴，事业有为的他宛如生活在天堂一般。受到纽约著名事务所的邀请，他携妻子来到纽约。奢华的生活近在眼前，一切似乎都昭示着明天的坦途与光明...')
--006：谍海计中计（The Recruit）
INSERT INTO [dbo].[Product]
           ([ProductId]
           ,[Name]
           ,[Genre]
           ,[Starring]
           ,[SupportingActors]
           ,[Director]
           ,[ScriptWriter]
           ,[ProductionCountry]
           ,[ProductionCompany]
           ,[ReleaseYear]
           ,[Language]
           ,[RunTime]
           ,[Price]
           ,[Poster]
           ,[Stock]
           ,[Story]
           ,[StoryAbstract])
VALUES	  ('006' 
           ,'谍海计中计（The Recruit）'
           ,'剧情|惊悚|动作'
           ,'阿尔·帕西诺|柯林·法瑞尔'
           ,'布丽姬·穆娜|盖布瑞·马赫特'
           ,'罗杰·唐纳森'
           ,'Roger Towne / 科特·维莫 / Mitch Glazer'
           ,'美国'
           ,'试金石影业公司'
           ,2003
           ,'英语 / 波斯语 / 俄语'
           ,115
           ,45
           ,'谍海计中计.jpg'
           ,10
           ,'这是一部由艾尔-帕西诺和科林-法瑞尔主演的超级惊险影片，影片情节扣人心弦，悬念重重。美国中央情报局那不太光彩的大门第一次敞开，让世人看到了其种种内幕：包括如何招募学员、如何进行间谍训练、如何在那个神秘的环境中求生。<p>也许詹姆斯-克莱顿 (科林-法瑞尔饰)当初并没有打算成为那样的学员，但他是美国最优秀的应届大学毕业生，于是也就成了沃尔特-伯克(艾尔-帕西诺饰)眼中当然的中情局间谍人选。詹姆斯也认为与普通人的生活相比还是执行中情局的任务更有意思，但是在成为一名情报官之前，他首先需要通过中情局秘密训练营地的考验，年轻的新学员们将在那里被打造成训练有素的间谍。随着伯克把间谍的种种游戏规则传授给詹姆斯，詹姆斯很快便开始崭露头角，同时他也渐渐迷恋上了自己的同学劳拉(布里奇特-莫伊纳汉饰)。</p><p>后来詹姆斯开始对自己的角色以及自己与老师之间猫与老鼠般的关系产生了疑虑，也就在这时伯克为他安排了一项调查“鼹鼠”的特殊任务。随着这一悬念越来越扣人心弦，观众们很快就会明白中情局那些老掉牙的套话并非戏言：“无人可以相信，无一不是假象”。</p>'
           ,'这是一部由艾尔-帕西诺和科林-法瑞尔主演的超级惊险影片，影片情节扣人心弦，悬念重重。美国中央情报局那不太光彩的大门第一次敞开，让世人看到了其种种内幕：包括如何招募学员、如何进行间谍训练、如何在那个神秘的环境中求生...')
GO
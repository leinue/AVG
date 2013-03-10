Olive AVG Engine 详细结构
===

Olive AVG Engine Project - [Homepage](https://github.com/leinue/AVG)

## Part 1 详细结构：图像呈现与事件捕捉模块（OAEDisplay）

> 该模块在写本文档前就已经被较好地完成了。但是由于结构变动，需要进行一定调整。这篇结构我写的充满了实现的味道，就不要吐槽了。


#### 1.模块中字段和主要数据类型

首先是本模块的全部字段：
> - `OAEDisplay` Class
> - *Public* Boolean: **IgnoreError** # 是否忽略异常
> - *Public* OAEItem: **ItemList()** # 储存Item的数组
> - *Public* Form: **GameForm** # 目标绘制窗口
> - Bitmap: **CacheBmp** # 缓存Bmp。
> - Graphics: **BmpGrap** # 从 **CacheBmp** 创建的Graphics对象，用于在缓存Bmp上绘制
> - Graphics: **FormGrap** # 从 **GameForm** 创建的Graphics对象，用于在窗口上直接绘制


由于本模块是以Item为单位绘制图像的，所以必须有 `OAEItem` 储存每个Item的信息。


`OAEItem`是一个类，结构如下：
> - `OAEItem` Class #用来储存一个Item的信息
> - **字段：**
> - String: **Name** #Item的名称
> - String: **Type** #Item的类型，分为Image和Text两种。
> - String: **Status** #Item的状态，为Normal，Click或Hover。
> - Rectangle: **Position** #Item的位置
> - Boolean: **Visible** #是否可见
> - OAETextItem: **Text** #Item显示的文字（仅当 **Type** 为Text时有效）
> - OAEImageItem: **Image** #Item显示的图像（仅当 **Type** 为Image时有效）
> - OAEEventCallArgs: **EventCallArgs** #Item产生事件时传递给 *脚本解析与执行模块* 的参数。
> - Boolean: **Available** # 无效或已销毁时该变量置为False。请在加载Item时将此标记置为True。
> - **方法：**
> - *Sub* : **Dispose**() # 销毁一个Item的一切资源，比如Image。

上面的结构中出现了几个新的类型，`OAETextItem`、`OAEImageItem`和`OAEEventCallArgs`。前两个打包了当Item分别为Text和Image时Item的文本和图像，后一个储存了当Item遇到事件时传递给 *脚本解析与执行模块* 的参数。
具体结构如下：

> - `OAETextItem` Structure # 储存Item作为Text时的文本内容
> - OAEItemText: **NormalText** # 在正常情况下显示的文本
> - OAEItemText: **HoverText** # 在鼠标悬浮情况下显示的文本
> - OAEItemText: **ClickText** # 在鼠标单击时显示的文本

-

> - `OAEImageItem` Structure # 储存Item作为Image时的图像
> - OAEItemImage: **NormalImage** # 在正常情况下显示的图像
> - OAEItemImage: **HoverImage** # 在鼠标悬浮情况下显示的图像
> - OAEItemImage: **ClickImage** # 在鼠标单击时显示的图像

-

> - `OAEItemText` Structure # 用来储存一个文本元素，包含字体、文本和特效。
> - String: **Text**
> - OAEFont: **Font**
> - OAETextEffect: **Effect**

-

> - `OAEItemImage` Structure # 用来储存一个图像元素，包含图像本身和特效。
> - Image: **Image**
> - OAEImageEffect: **Effect**

-

> - `OAEEventCallArgs` Structure # 用来储存一个Item在遇到事件时调用 *脚本解析与执行模块* 中事件处理函数的参数
> - Function: **InvokeFunction**(String: **EventArgs**) # 接受事件的函数（VB中用那个费死进的委托解决）
> - String: **OnClick** # 当Item被点击时
> - String: **OnHover** # 当鼠标悬浮在Item上时

`OAEFont` 把Font和Brush捆绑了起来，方便传递。具体结构如下：

> - `OAEFont` Structure # 用来捆绑写文本所需的Brush，Font。
> - Font: **Font** # 用来写文本的字体
> - SolidBrush: **Brush** # 用来写文本的笔刷

`OAETextEffect` 是一个储存文字特效信息的结构。目前为止特效主要就是阴影。之后会扩展出更多新特效。具体结构如下：

> - `OAETextEffect` Structure # 文字的特效信息
> - OAEEffectShadow: **Shadow** # 文字阴影特效设置

`OAEEffectShadow` 是有关文字阴影特效的设置。同样可用于图像。

> - `OAEEffectShadow` Structure
> - Boolean: **Enable** # 是否启用该特效
> - Integer: **Offset** # 阴影的偏移，以像素为单位
> - SolidBrush: **Brush** # 画阴影的笔刷


`OAEImageEffect` 是一个储存图像特效信息的结构，与 `OAETextEffect` 很相似。具体结构如下：

> - `OAEImageEffect` Structure # 图像的特效信息
> - OAEEffectShadow: **Shadow** # 图像阴影特效设置
> - OAEEffectTransp: **Transparent** # 图像的半透明设置

-

> - `OAEEffectTransp` Structure
> - Boolean: **Enable** # 是否启用该特效
> - Integer: **Transparent** # 透明度

####2.本模块中的方法

> - `OAEDisplay` Class

> - *Construct* *Sub* : **New** (Form : & **Form** ) # 给 **GameForm** 赋值，建立各种事件与本模块中方法的链接。

> - *Sub* : **Init** (Integer : **Width** ,Integer : **Height**) # 通过窗体宽和高初始化 **CacheBmp** ，重置 **ItemList** ，同时初始化 **FormGrap** 和 **BmpGrap** 。

> - *Public Sub* : **PaintForm** () Handle Form.Paint  # 往 **GameForm** 上绘制整个 **ItemList**。

> - *Private* *Sub* : **DrawItem** (Graphics: & **Grap** , OAEItem: & **Item** ) # 往 **Grap** 上绘制所给的Item

> - *Private* *Sub* : **DrawText** (Graphics: & **Grap** ,String: **Text** , OAETextEffect: **Effect** ) # 往 **Grap** 上绘制文本

> - *Private* *Sub* : **DrawImage** (Graphics: & **Grap** ,Image: & **Image** ) # 往 **Grap** 上绘制文本

> - *Private* *Function* : **ImageApplyTransp** (Integer: **Transparent** ) ImageAttrbutes # 通过透明度创建ImageAttrbutes *（这个函数是临时放这里的，如果将来有别的特效，会重写特效方面函数）*

> - *Sub* : **EventOccur** (OAEItem: **Item** ,String: **EventType** ) # 调用 *脚本解析与执行模块* 中处理Event的函数，并将Item中的**EventCallArgs**中的参数传递给处理函数。

> - *Sub* : **eMouseMove** (System.object: **sender** ,System.Windows.Forms.MouseEventArgs: **e** ) Handle Form.MouseMove # 处理窗体鼠标移动事件的方法，负责检查鼠标移动对每个Item的影响，负责选择性重绘，并且会调用 **EventOccur** 。

> - *Sub* : **eMouseDown** (System.object: **sender** ,System.Windows.Forms.MouseEventArgs: **e** ) Handle Form.MouseDown # 处理窗体鼠标点击事件的方法，负责检查鼠标移动对每个Item的影响，负责选择性重绘，并且会调用 **EventOccur** 。

> - *Sub* **Dispose** () Handle Form.Closing # 销毁一切Item，Graphics，Bitmap并释放资源。

> - *Sub* : **AddItem** (OAEItem : **Item** ) # 把一个Item加入ItemList。如果有现有Item与该Item重名，则替换旧的Item。

> - *Sub* : **GetItem** (String : **ItemName**) # 通过Item名称获取一个Item。

> - *Sub* : **DeleteItem** (String : **ItemName** ) # 销毁一个Item并释放资源。空Item仍保存在ItemList中。

> - *Sub* : **ResetItemList** () # 清除ItemList中任何元素并且释放资源。

#### 3.如何使用该模块

*注：出于效率考虑，该模块只会在窗口出现OnPaint事件时自动重绘，在其他任何情况下，如果没有调用FormPaint方法，窗口不会重绘。*

##### 初始化时

1. 首先创建该对象，创建时把游戏窗体传递给构造函数。
2. 然后利用窗体的宽和高的设定调用 **Init** 函数，初始化缓存Bmp，Graphics。
3. 利用AddItem函数加入Item。Item的详细格式，参见本文档第一部分。

##### 过程中添加/删除Item

1. 调用AddItem，添加Item。（调用DeleteItem删除Item）
2. 调用FormPaint，重绘界面。

##### 过程中修改Item

1. 调用GetItem获取旧Item。
2. 修改Item。（基于.Net的特性，对象会被以引用的形式传递。）
4. 调用FormPaint，重绘界面。

##### 清除界面准备重新绘制

1. 调用Init函数重置ItemList。
2. 重新添加Item。
3. 调用FormPaint，重绘界面。

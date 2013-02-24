Olive AVG Script 设计
===

Olive AVG Engine Project - [Homepage](https://github.com/leinue/AVG)

## Part 1 概述

Olive AVG Script，简称OAS，是一种脚本语言。其作用是驱动OAE，呈现一个冒险游戏。这种脚本语言拥有基本的控制结构（if...else，while），可以定义变量，亦可以定义类似于函数的“Action”。这种脚本非常灵活，但是由于没有过多的大包大揽，不能提供用户极其良好的用户体验（但我觉得还是比ks好点），但绝对是可以处理很多情况的。

本文档是为入门学习这种脚本的人写的，但有一些编程基础能够帮助更好理解。

## Part 2 入门

OAS由一系列Action组成，并且包含了对游戏界面的定义。具体格式如下：

> File: Main.oas

    #* Olive AVG Script V2.0

    [Action]Entry{
        Do[Action]LoadScene:Main #加载名为Main的场景
    }

    [DScene]Main{
        @Width:800
        @Height:600
        [Event]OnLoad:Do[Action]ShowItem:[DItem]Item1,[DItem]Item2,[DItem]Item3
    }

在这段代码中，第一行是脚本的版本信息，证明这是一个版本为2.0的OAS。  
第三行的`[Action]Entry`，定义了一个名为Entry的Action。后面紧跟着的一对花括号中包裹的代码就是这个Action的具体操作。花括号在OAS的各种结构上都有应用，这一点是参照C语言的。

所有OAE脚本的入口点在一个名为“Entry”的Action上。脚本读取完后会首先执行 `Do[Action]Entry` 。  

`Do[Action]LoadScene:[DScene]Main`意为执行名为LoadScene的Action，并且把`Main`作为参数传递过去。大致意思就是“显示一个名为Main的场景。”

“#”后到换行符前的内容会被认定是注释，不会被解析。

接下来的`[DScene]Main`，定义了一个名为Main的场景。花括号内的内容是对场景Main各种属性的定义。
`@Width:800` 和 `@Height:600` 意为定义Main场景的宽度（即游戏窗口的宽度）的为800，高度为600。

`[Event]OnLoad:Do[Action]ShowItem:[DItem]Item1,[DItem]Item2,[DItem]Item3` 意为定义在该场景产生OnLoad事件时执行的操作。当Main在加载的时候，OnLoad事件就会触发，OAE就会执行所定义的操作： `Do[Action]ShowItem:[DItem]Item1,[DItem]Item2,[DItem]Item3` 。

“ShowItem”是一个用来显示DItem的Action（动作）。DItem是游戏界面的基本显示单元。这里并没有详细介绍Item1，Item2和Item3的定义，这些本文之后还会有所介绍。

## Part 3 深入OAS基本语法

回顾一下我们的第一段代码，在每一行具有实际运行意义的代码都会有两样东西（空行，花括号和注释不计其中），一个是操作名称，一个是操作类型。第三行的`[Action]Entry`，或写作`Def[Action]Entry`，操作名称是 `Def` ，操作类型是 `[Action]`，操作对象是`Entry`。没有操作名称的一行具有实际运行意义的代码，会被自动补全一个操作名称 `Def`，意思就是Define，定义。
这句代码意在定义一个`[Action]`。Do则代表着执行一个 `[Action]`。在这行代码中，操作名称是 `Do` ，操作类型是 `Action` ，操作对象是 `ShowItem` ，操作参数是 `[DItem]Item1`， `[DItem]Item2` 和 `[DItem]Item3` 。并不是每一行具有实际运行意义的代码都有操作对象和操作参数。

在OAS中，操作任何东西，在操作对象前必须带有操作类型，并且每行代码前有必须有一个操作名称。这里的操作名称被省略，OAE默认补全为Def。

在 `[DScene]Main{` 中，定义的是一个场景。DScene中D是Design的简写。
`@Width:800` 中的 `@`在这里是`[Attr]` （Attribute）的简写。冒号代表赋值。

## Part 4 定义/操作变量与表达式

在OAS中，变量没有明确的数据类型。定义与操作变量的一个简单的例子如下：

    @Hello:10

    [Action]AddOne{
        @Hello:@Hello+1
		Do[Action]DebugOutput:@Hello #输出“11”
    }

在这段代码中，先定义了一个名为Hello，值为10的变量。执行AddOne，就会自动给Hello加上1。 `@Hello+1` 是一个表达式。同样，第一行的`10` 也是一个表达式。`@` 在这里被自动转换为 `[Var]` （与DScene中默认转换为 `[Attr]` 不同）。在表达式运算中，解析器会先用变量的值替换变量本身，再进行计算（上文的 `@Hello+1` 会先被转换为 `10+1` ）。
这里的 `Do[Action]DebugOutput:@Hello` 的意思是在调试窗口输出 `@Hello` 的值。 `[Action]DebugOutput` 在后文中还会被经常用到。

表达式计算的顺序与四则运算的顺序相同。可以使用括号改变运算顺序：  

    (1+2)*@Hello-(3+4)

在表达式运算中，加减乘除运算符和其他语言一样，由“+”，“-”，“*”，“/”表示。

除了整数以外，变量也可以是浮点数（小数），表达式也可以出现浮点数：

    @Hello:0.5
    @World:@Hello*0.3

变量也可以是字符串，但是作为字符串的变量不能进行加减乘除的算术操作。字符串必须被双引号括起。“＋”运算符在操作字符串变量时会起到连接作用。如果一个字符串加上一个数字，那么就把这个数字转换为字符串再进行连接操作。

    @Hello:"Hello " # @Hello is "Hello "
    @Hello:@Hello + "World!" # @Hello is "Hello world!"
    @Hello:@Hello + 3 # @Hello is "Hello world!3"

对存放字符串的变量进行“-”，“*”，“/”操作会报错。

表达式可以作为Action的参数传递。在传递过程中，会先计算表达式的值。

## Part 5 If...else 语句

If...else是OAS中的判断结构。下面是一个简单的演示：

    @Hello:5
    @World:0
    
    [If]:@Hello = 5 {
        @World:1
    }

	Do[Action]DebugOutput:@World # 输出“1”

代码中 `If:@Hello = 3 ` 的意思是判断 `@Hello` 是否等于 3。如果 `@Hello` 等于 5，条件成立，就执行花括号中的代码，就会把 `@World` 设置为1；如果所给的条件不成立，那么就跳过花括号中的代码，继续运行。假设 `@Hello` 的值是3，那么 `@Hello` 等于 5 这条条件不成立，所以花括号中把 `@World` 设置为1的语句就不会执行，最后 `@World` 的值还是0，所以最后一行的调试输出是0。

那么加上Else呢？

    @Hello:6
    @World:0
    
    [If]:@Hello = 5 {
        @World:1
    }
    [Else]{
        @World:2
    }

	Do[Action]DebugOutput:@World # 输出“2”

在上面的语句中，Else的意思是，如果上面If中的条件不成立，就执行Else花括号所括的代码。在上面的程序中， `@Hello` 的值是6，所以 `@Hello` 等于 5 不成立。所以If后面花括号的代码被跳过，执行Else花括号的代码，最后 `@World` 的值是2，调试输出显示2。

上述代码中， `=` 是一个判断操作符，`If @Hello = 5` 的意思是判断`@Hello`是否等于5。同样，判断操作符还有 `>` `<` `>=` `<=`。

`@Hello = 3` 也是一种表达式，当条件成立时，这个表达式得1，反之得0。

另外， `If:@Hello = 5` 和 `If:@Hello=5` 执行起来是等效的。空格不会影响解析。

## Part 6 Action的定义

Action可以被翻译为“动作”，是类似函数的一种结构（起“Action”这个名字主要是为了接近剧本中的用词）。可以有参数，同时也可以有返回值。下面一个最简单的Action的定义：

    [Action]SayHello{
		Do[Action]DebugOutput:"Hello"
		Do[Action]DebugOutput:" World"
    }

在定义完这个名为Add的Action后，如果我使用 `Do[Action]SayHello` 调用这个Action的话，解析器就会进入“Add”内部，执行花括号内所包含的代码，就会在调试窗口输出“Hello World”。

下面再来看一个带参数的Action的定义：

	[Action]Add:@A,@B{
		@C : @A + @B
		Do[Action]DebugOutput:@C
    }

这个时候如果执行 `Do[Action]Add:10,20` ，10和20就会被当做两个参数传递给Add，就相当于在Add内部定义了两个变量：`@A=10`，`@B=20`。执行后调试窗口会输出"30"。当然，直接把函数内部（花括号内代码）写作 `Do[Action]DebugOutput:@A+@B`也是可以的。

下面再看一个拥有返回值的函数：

	[Action]Add:@A,@B{
		@C : @A + @B
		Return:@C
    }

	Do[Action]DebugOutput:Do[Action]Add:10,20 #输出“30”

Return是返回标记，解析器执行到Return后，会返回冒号后面的表达式的值。利用刚才的例子中的Add函数，计算如下的表达式：

	@C : (Do[Action]Add:10,20) - 10

解析器会先计算出 `Do[Action]Add:10,20` 的值，把表达式转化成

	@C : 30 - 10

最后 `@C` 的值为20。


然而不要以为上面的括号没什么作用。如果不加括号，上面的表达式会按照如下计算顺序进行：

	@C : Do[Action]Add:10,(20 - 10)

逗号优先级比减号低。
Olive AVG Script 设计
===

Olive AVG Engine Project - [Homepage](https://github.com/leinue/AVG)

## Part 1 概述

Olive AVG Script，简称OAS，是一种脚本语言。其作用是驱动OAE，呈现一个冒险游戏。这种脚本语言拥有基本的控制结构（if...else，while），可以定义变量，亦可以定义类似于函数的“Action”。这种脚本非常灵活，但是由于没有过多的大包大揽，用户体验可能不会很好，但绝对是可以处理很多情况的。

## Part 2 入门

OAS由一系列Action组成，并且包含了对游戏界面的定义。具体格式如下：

> File: Main.oas

    #* Olive AVG Script V2.0

    <Action>Entry{
        Do<Action>LoadScene:Main #加载名为Main的场景
    }

    <DScene>Main{
        @Width:800
        @Height:600
        <Event>OnLoad:Do<Action>{
            Do<Action>ShowItem:<DItem>Item1,<DItem>Item2,<DItem>Item3
        } 
    }

在这段代码中，第一行是脚本的版本信息，证明这是一个版本为2.0的OAS。  
第三行的`<Action>Entry`，定义了一个名为Entry的Action。后面紧跟着的一对花括号中包裹的代码就是这个Action的具体代码。花括号在OAS的各种结构上都有应用，这一点是参照C语言的。

所有OAE脚本的入口点在Entry。脚本读取完后会首先执行Entry。  

`Do<Action>LoadScene:<DScene>Main`意为执行名为LoadScene的Action，并且把`Main`作为参数传递过去。

“#”后的内容会被认定是注释，不会被解析。

接下来的`<DScene>Main`，定义了一个名为Main的场景。花括号内的内容是对场景Main各种属性的定义。
`@Width:800` 和 `@Height:600` 意为定义Main场景的宽度（即游戏窗口的宽度）的为800，高度为600。

`<Event>OnLoad:Do<Action>{` 意为定义在该场景产生OnLoad事件时执行的代码。当Main在加载的时候，OnLoad事件就会触发，OAE就会执行所定义的代码。

“ShowItem”是一个用来显示DItem的Action。DItem是游戏界面的基本显示单元。这里并没有详细写Item1，Item2和Item3的定义，这些本文之后还会有所介绍。

## Part 3 深入OAS基本语法

回顾一下我们的第一段代码，在每一句代码都会有两样东西，一个是操作名称，一个是操作类型。第三行的`<Action>Entry`，或写作`Def<Action>Entry`，操作名称是 `Def` ，操作类型是 `<Action>`，操作对象是`Entry`。这句代码意在定义一个`<Action>`。Do则代表着执行一个`<Action>`。Do的操作对象必须是`<Action>`。而对于`Do<Action>ShowItem:Item1,Item2,Item3`，操作名称是 `Do` ，操作类型是 `Action` ，操作对象是 `ShowItem` ，操作参数是 `Item1`， `Item2` 和 `Item3`

在OAS中，操作任何东西，在操作对象前必须带有操作类型，并且每句代码前有必须有一个操作名称。这里的操作名称被省略，OAE默认补全为Def。

在 `<DScene>Main{` 中，定义的是一个场景。DScene中D是Design的简写。
`@Size:100,200` 中的 `@`在这里是`<Attr>` （Attribute）的简写。冒号代表赋值。

这部分也可以写为：  

    <DScene>Main{
        @Width:800
        @Height:600
        <Event>OnLoad:Do<Action>ShowItem
    }

    <Action>ShowItem{
        Do<Action>ShowItem:<DItem>Item1,<DItem>Item2,<DItem>Item3
    }

`Do<Action>{` 直接定义了一个临时的Action，然后执行了这个Action。这个Action一经执行就被销毁。这一点在之后还会有所提及。

## Part 4 定义/操作变量与表达式

在OAS中，变量没有明确的数据类型。定义与操作变量的一个简单的例子如下：

    @Hello:10

    <Action>AddOne{
        @Hello:@Hello+1
    }

在这段代码中，先定义了一个名为Hello，值为10的变量。执行AddOne，就会自动给Hello加上1。 `@Hello+1` 是一个表达式。同样，第一行的`10` 也是一个表达式。`@` 在这里被自动转换为 `<Var>` 。

表达式计算的顺序与四则运算的顺序相同。可以使用括号改变运算顺序：  

    (1+2)*@Hello-(3+4)

在表达式运算中，加减乘除运算符和其他语言一样，由“+”，“-”，“*”，“/”表示。

除了整数以外，变量也可以是浮点数（小数），表达式也可以出现浮点数：

    @Hello:0.5
    @World:@Hello*0.3

变量也可以是字符串，但是作为字符串的变量不能进行加减乘除的算术操作。字符串必须被双引号括起。“＋”运算符在操作字符串变量时会起到连接作用。

    @Hello:"Hello "
    @Hello:@Hello + "World!" # 现在Hello的值是"Hello world!"
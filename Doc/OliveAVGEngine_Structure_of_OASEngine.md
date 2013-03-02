Olive AVG Script Engine 结构
===

Olive AVG Engine Project - [Homepage](https://github.com/leinue/AVG)

## Part 1 OASEngine

整个OAS各个组件的集中控制模块。

> `OASEngine` Class  
> **字段：**   
> *OASDataStack*: **DataStack** #全局数据储存模块

工作主要流程：

1. 读入脚本，将脚本转换成一个OpBlock。
2. 调用OASOp中的DoOpBlock，将DataStack传递过去，令其解读第一步返回的OpBlock并且将数据全部储存入Data。此时，在DoOpBlock中执行过的代码不会再继续存入DataStack。

## Part 2 OASOperation

用来处理OASOperation的模块。

> `OASOperation` Class  
> **字段：**  
> 无。  
> **方法：**  
> *Public Sub* **DoOpBlock** ( *OpBlock* , *OASDataStack* ) # 执行一个OpBlock，将改动写入所提供的OASDataStack。  
> *Public Sub* **DoOpSingle** ( *OpSingle* , *OASDataStack* ) # 执行一条OpSingle，将改动写入所提供的OASDataStack。  
> *Public Function*  **GetPackedOp** ( *StreamReader* ) *OpBlock* # 将文本格式的脚本打包成Op，并返回一个OpBlock。

#### 3.1 OASOperation中的Structure

> `OpSingle` Structure # OAS中的单条操作  
> **字段：**  
> *String*: **Op** # 操作名称，例如Do、Def、If  
> *String*: **TarType** # 操作目标类型，如Action、Var  
> *Parameter*: **Parameter[]** # 一个保存该操作参数的数组。

---

> `OpBlock` Structure # OAS中的一个操作块，例如Action块，If块  
> **字段：**  
> *String*: **Op** # 操作名称，例如Do、Def、If  
> *String*: **TarType** # 操作目标类型，如Action、Var
> *Parameter*: **Parameter[]** # 一个保存该操作参数的数组。  
> *OpPtr*: **Operations[]** # 一个保存代码块中全部操作的指针的数组

---

> `OpPtr` Structure # 指向OpList中单个操作或者单个代码块的指针  
> **字段：**  
> *String*: **Type** # Single or Block  
> *Integer*: **Offset** # 在数组中的位置（下标）

---

> `OpList` Structure # 一个包装了OpBlock数组和OpSingle数组的结构  
> **字段：**  
> *OpBlock*: **OpBlock[]**  
> *OpSingle*: **OpSingle[]**  
> *Int*: **OpBlockLength**  
> *Int*: **OpSingleLength**  
> **方法：**  
> *Public Function* **AddOpSingle** ( *OpSingle* ) *OpPtr*  
> *Public Function* **AddOpBlock** ( *OpBlock* ) *OpPtr*  

---

> `Parameter` Structure # 用于储存一个操作的参数（形参或者实参）  
> **字段：**   
> *String*: **DefParaN[]** # 定义时的形参名称   
> *String*: **DefParaT[]** # 定义时的形参类型 
> *ParaOnCall*: **CallPara[]** # 调用时的参数表达式

---

> `ParaOnCall` Structure # 储存Action调用时的实参  
> *String*: **Type** # 实参的类型  
> *String*: **Name** # 实参的名称  
> *OASExpr*: **Expr** # 实参的表达式  

## Part 3 OASExpr

用于储存和处理OAS中的表达式。




## Part 4 OASDataStack

保存脚本执行过程中的数据。例如OpList，Action，Var等。

> `OASDataStack` Structure # 数据栈管理机制。具有push和pop的功能  
> *Int*: **Ptr** # 当前操作的数据栈数组的下标。  
> *OpList*: **OpList** # 储存运行过程中的全部Op。  
> *OASData*:　**Data[]** # 数据栈的核心，随花括号嵌套出栈入栈。当Ptr为0时不再退栈。

> `OASData` Structure  
> **字段：**  
> *Action*: **ActList[]** # 储存运行中定义的Action。  
> *Var*: **VarList[]** # 储存运行中定义的Var。  
>　*OpPtr*: **EventCallOp** # 储存当用户事件发生时执行的操作。  
> **方法：**  
> *Function* **DoAction** ( *String* Name , *ParaOnCall* Para[] ) *String* ReturnVal # 执行指定的Action。  
> *Sub* **AddAction** ( *String* Name , *OpPtr* Ptr ) # 把Ptr所指的Action代码块加入ActList。
> *Function* **GetVar** ( *String* Name ) *String* VarVal # 获取名为Name的变量的值。  
> *Sub* **SetVar** ( *String* Name ， *String* Val ) # 把名为Name的变量的值设置为Val。如果变量不存在，则新建一个。

## 附录 术语

#### 1.Operation（Op），OpSingle，OpBlock
OAS中脚本语句的执行单位。脚本将被读入时将会被处理成Operation的形式。OAS最小的执行单位是OpSingle，意思是单条操作。OpBlock是一组OpSingle的集合，用于储存Action，If之类的模块。
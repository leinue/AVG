Olive AVG Script Engine 结构
===

Olive AVG Engine Project - [Homepage](https://github.com/leinue/AVG)

## Part 1 OASEngine

整个OAS各个组件的集中控制模块。

> `OASEngine` Class  
> **字段：**   
> *OASDataStack*: **DataStack** # 数据栈，实现OAS的栈机制  
> *OASOperation*: **Operation** # 储存脚本中的全部指令  
> *OpPtr*: **EventCallOp[]** # 储存当用户事件发生时执行的操作。每个Event全局有效。 

工作主要流程：

1. 读入脚本，将脚本转换成一个OpBlock。
2. 调用OASOp中的DoOpBlock.

## Part 2 OASOperation

用来处理OASOperation的模块。

> `OASOperation` Class  
> **字段：**  
> *OASData*: **Data**     
> **方法：**  
> *Public Sub* **New** ( *OASDataStack* )  # 构造函数  
> *Public Sub* **DoOpBlock** ( *OpBlock* , *ParaOnCall* ) # 执行一个OpBlock，将改动写入所提供的OASDataStack。  
> *Public Sub* **DoOpSingle** ( *OpSingle* ) # 执行一条OpSingle，将改动写入所提供的OASDataStack。  
> *Public Function*  **LoadScript** ( *StreamReader* ) *OpBlock* # 将文本格式的脚本打包成Op，存入Data，并执行一遍（定义Action……）。

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

> `OpTable` Structure # 一个包装了OpBlock数组和OpSingle数组的结构  
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
> *ParaOnDef*: **DefPara[]** # 定义时的形参列表  
> *ParaOnCall*: **CallPara[]** # 调用时的实参列表

---

> `ParaOnCall` Structure # 储存Action调用时的实参  
> *String*: **Type** # 实参的类型  
> *String*: **Name** # 实参的名称  
> *OASExpr*: **Expr** # 实参的表达式  

---

> `ParaOnDef` Structure # 储存定义Action时的形参  
> *String*: **Name**  
> *String*: **Type**  

## Part 3 OASExpr

用于储存和处理OAS中的表达式。

> `OAEExpr` Structure  
> **字段：**  
> *OASDataStack*: **DataStack**  
> *ExprE*: **ExprElement[]** # 用于储存表达式的单个元素  
> *ExprB*: **ExprBlock[]** # 用于储存一个括号的内容  
> **方法：**  
> *Sub*: **CreateByString** ( *String* , *OASDataStack* ) 通过字符串创建一个表达式  
> *Function*: **Caculate** ()

---

> `ExprB` Structure

## Part 4 OASData

保存脚本执行过程中的数据。例如OpList，Action，Var等。

> `OASData` Structure  
> **字段：**  
> *OpTable*: **OpTable** # 储存运行过程中的全部Op。 
> *OASVarTable* ： **VarTable** # 用于存放每个Var的真实数据。该类型不储存Var名称，只储存Var的值。通过Index可以访问这里的内容。
> *OASDataStack* : **DataStack** # 数据栈，用于实现Action的栈机制。
> **方法：**  
> *Function* **GetVar** ( *String* Name ) *String* VarVal # 获取名为Name的变量的值。  
> *Sub* **SetVar** ( *String* Name ， *String* Val ) # 把名为Name的变量的值设置为Val。如果变量不存在，则新建一个。

> `OASDataStack` Structure # 数据栈管理机制。具有push和pop的功能  
> *Int*: **Ptr** # 当前操作的数据栈数组的下标。  
> *StackElement*:　**Data[]** # 数据栈的核心，随花括号嵌套出栈入栈。当Ptr为0时不再退栈。

> `StackElement` Structure # 每个栈元素  
> **字段：**  
> *OASActionL*: **ActList** # 储存运行中定义的Action。  
> *OASVarL*: **VarList** # 储存运行中定义的Var。

## Part 5 OASActionL & Action

> `OASActionL` Structure # OAS Action List  
> **字段：**  
> **方法：**  
> *Function* **DoAction** ( *String* Name , *ParaOnCall* Para[] ) *String* ReturnVal # 执行指定的Action。并且映射参数。  
> *Sub* **AddAction** ( *String* Name , *OpPtr* Ptr ) # 把Ptr所指的Action代码块加入ActList。

## Part6 Var & OASVarL

> `OASVarL` Structure # 这个Structure放在DataStack中。  
> **字段：**  
> *VarPtr* : **Ptr**[] # 指向Var的指针们  
> **方法：**  
> *Public Function* **GetVarPtr** ( *String* VarName ) # 获得一个Var在VarTable中的指针。
> *Public Function* **GetVarDataByName** ( *String* VarName ) # 获得一个Var的数据。

> `OASVarTable` Structure # 这个Structure直接放在Data类中。  
> **字段：**  
> *Var* : **Vars**[] # 存放Var的数组。  
> **方法：**  
> *Public Function* **GetVarDataByPtr** ( *Integer* Index )

> `Var` Structure  
> **字段：**  
> *String* : **Type** # 该Var的类型  
> *String* : **Data** # 该Var的数据
> *Boolean* : **IfRef** # 该Var是否是引用指针。这个字段目的是在退栈时把该层所有var的available置为False时，辨认是否该Var是引用，若是，则不销毁所指向的内容。

> `VarPtr` Structure  
> **字段：**  
> *String* : Name  
> *Integer* : Index # 在VarTable中的索引  
> **方法：**  
> *Public Function* **GetVarData** () *Var* # 获得一个Var的数据。


## 附录 术语

#### 1.Operation（Op），OpSingle，OpBlock
OAS中脚本语句的执行单位。脚本将被读入时将会被处理成Operation的形式。OAS最小的执行单位是OpSingle，意思是单条操作。OpBlock是一组OpSingle的集合，用于储存Action，If之类的模块。
# SqlXmlMapper
针对DbConnection扩展读取Xml文件Sql语句，语法格式和遵循Ado.Net原生的Sql写法

### 说明
本框架核心代码摘自[Insql](https://rainrcn.github.io/insql/#/),Insql定位是ORM,可以完全独立的引入它。SqlXmlMapper定位于基于Xml文件Sql语句扩展。所以很多场景并不冲突。作者只是以为单独引入ORM有点过于笨重，所以基于Insql精简了一下核心操作。

### 使用文档
笔者已将具体的使用文档放到了github中,可以下载[SqlXmlMapper使用说明.docx](https://github.com/softlgl/SqlXmlMapper/blob/main/SqlXmlMapper%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E.docx)进行详细的查看或查看源码了解相关使用。**想了解详细的用法可以查看[测试项目](https://github.com/softlgl/SqlXmlMapper/blob/main/SqlXmlTest/Program.cs)使用示例**。

#### 使用场景
针对业务场景中复杂查询的情况，使用EF虽然可以满足实现功能但是无法保证sql语句的可预料性。所以引入了将复杂Sql查询语句的xml化，编写规则符合ado.net的编程习惯，使用也比较简单。

#### 引入方式
需要使用的项目中引入IServiceCollection扩展方法AddSqlXmlMapper，其中传递的参数为sqlxml所在的文件夹路径
```cs
IServiceCollection services = new ServiceCollection();
services.AddSqlXmlMapper("./SqlMapper");
```
支持传递多个xml文件夹路径
```cs
IServiceCollection services = new ServiceCollection();
services.AddSqlXmlMapper("./SqlMapper","./SqlMapper2","./SqlMapper3");
```

##### 编写SqlXml
①Sql语句是存放到上文配置的xml文件夹中的（xml文件要在vs上配置为始终复制）。
②Xml文件的名称必须以xml节点中insql type的值开头
```xml
<?xml version="1.0" encoding="utf-8" ?>
<insql type="CoreMysql.Program" >

  <select id="GetStudentCount">
    select Id,Name from Student
    where Deleted=@Deleted
    group by Id,Name
    having <![CDATA[count(Name)>=@NameCount]]>
  </select>

  <select id="GetStudents">
    select Id,Name from Student
    <where>
      Deleted=@Deleted
      <if test="Name!=null">
       and Name=@Name
      </if>
    </where>
  </select>
  
</insql>
```
 
③目前支持多个xml共享一个命名空间type的形式，但是必须保证其中Type+SqlId组成Sql节点唯一标识
 
支持动态where和if判断，sql语句的形式和原生的ado.net写法保持一致即可
如果不需要使用动态条件判断的时候直接写sql语句即可
 

##### 使用DbContext扩展
笔者在测试文件中写了一个关于EF查询的扩展示例[DbContextExtensions.cs](https://github.com/softlgl/SqlXmlMapper/blob/main/SqlXmlTest/DbContextExtensions.cs)用于演示，具体使用方式如下
①泛型第一个参数为命名空间即和xml里的声明的type保持一致，标识一组sql的标识
比如这里可以是我们项目中的某个具体的Repository
②泛型的第二个参数表示查询返回结果对应的实体类型
③参数列表第一个表示IServiceProvider实例，第二个参数表示Sql的唯一标识Id，第三个参数表示要传递的参数查询条件
如果是在一个Repository中想查询别的Repository命名空间下的sql,但是并不像直接引入这个Repository类，可以直接通过参数传递命名空间

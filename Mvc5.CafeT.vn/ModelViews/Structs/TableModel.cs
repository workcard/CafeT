using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.ModelViews.Structs
{
    public class Cell
    {
        public string Value { set; get; }
        public int Index { set; get; } = 0; //Index in row
        public Cell(string value, int index = 0)
        {
            Value = value;
            Index = index;
        }
    }

    public class Row
    {
        public int Length { set; get; } = 0;
        public List<Cell> Cells { set; get; }
        public Row(int length)
        {
            Length = length;
        }
        public Row(string[] items)
        {
            Length = items.Length;
            for(int i = 0; i<Length; i++)
            {
                Cell _cell = new Cell(items[i], i);
                Cells.Add(_cell);
            }
        }
    }

    public class TableModel
    {
        public string Name { set; get; }
        public List<Row> Rows { set; get; }
        public TableModel(string name)
        {
            Name = name;
        }
        public void AddRow(Row row)
        {
            Rows.Add(row);
        }
        public void BuildTitle()
        {
            Row _row = new Row(31);
            List<string> list = new List<string>();
            for(int i=0; i < 31; i++)
            {
                list.Add((i + 1).ToString());
            }
        }
        public void BuildRows()
        {
            BuildRow(0);
        }
        public void BuildRow(int i)
        {

        }
        public void Build()
        {
            BuildTitle();
            BuildRows();
        }
        public void Print()
        {
        }
    }
}
using UnityEditor.ImmediateWindow.Services;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.ImmediateWindow.UI
{
    internal class ClassType : ValueInspector
    {
        private object CurrentObject { get; set; }
        private bool ShowValue { get; set; }
        
        private VisualElement LabelRow { get; set; }        
        private CollapsedPropertyGroup CollapsedPropertyGroup { get; set; }
        private ExpandedPropertyGroup ExpandedPropertyGroup { get; set; }
        private Span PinnedItem { get; set; }
        private string PinnedSymbol { get; set; }
        private Arrow Arrow { get; set; }

        public ClassType(object obj, bool showValue = true)
        {
            PinnedItem = new Span();
            PinnedItem.AddToClassList("pinned");
            PinnedItem.RegisterCallback<TooltipEvent>(e => {
                e.rect = PinnedItem.worldBound;
                e.tooltip = string.Format("Use {0} to refer to this element", PinnedSymbol);
                e.StopImmediatePropagation();
            });
            
            LabelRow = new VisualElement();
            LabelRow.AddToClassList("row");
            Add(LabelRow);
            LabelRow.RegisterCallback<MouseDownEvent>(OnClick);                

            // Secondary objects are not clickable
            if (showValue)
            {
                Arrow = new Arrow(Arrow.Direction.Right);
                LabelRow.Add(Arrow);
            }

            SetObject(obj, showValue);
        }

        private void OnClick(MouseDownEvent evt)
        {
            ToggleExpand();
        }

        void ToggleExpand()
        {
            CurrentState = CurrentState == State.Collapsed ? State.Expanded : State.Collapsed;
            Arrow.SetDirection( CurrentState == State.Collapsed ? Arrow.Direction.Right : Arrow.Direction.Down);
            Refresh();
        }

        public void SetObject(object obj, bool showValue = true)
        {
            CurrentObject = obj;
            ShowValue = showValue;

            var typeLabel = new Span(CurrentObject.GetType().Name);
            typeLabel.AddToClassList("typename");
            typeLabel.RegisterCallback<TooltipEvent>(e => {
                e.rect = typeLabel.worldBound;
                e.tooltip = CurrentObject.GetType().FullName;
                e.StopImmediatePropagation();
            });
            
            if (showValue)
                typeLabel.RegisterCallback<MouseDownEvent>(OnTypeClick);

            LabelRow.Add(typeLabel);

            Refresh();
        }
        
        void Refresh()
        {
            if (ShowValue && LabelRow.Contains(PinnedItem))
                LabelRow.Remove(PinnedItem);

            if (CurrentState == State.Collapsed)
            {
                if (ExpandedPropertyGroup != null)
                    Remove(ExpandedPropertyGroup);

                if (ShowValue)
                {
                    CollapsedPropertyGroup = new CollapsedPropertyGroup(CurrentObject);
                    LabelRow.Add(CollapsedPropertyGroup);
                }
            }
            else
            {
                if (CollapsedPropertyGroup != null)
                    LabelRow.Remove(CollapsedPropertyGroup);

                ExpandedPropertyGroup = new ExpandedPropertyGroup(CurrentObject);
                Add(ExpandedPropertyGroup);
            }
            
            if (ShowValue)
                LabelRow.Add(PinnedItem);
        }

        async void OnTypeClick(MouseDownEvent evt)
        {
            PinnedSymbol = await Evaluator.Instance.AddToNextGlobal(CurrentObject, () =>
            {
                PinnedSymbol = "";
                PinnedItem.text = "";
            });

            PinnedItem.text = string.Format("== {0}", PinnedSymbol);
        }
    }
}
rawids = File.readlines('input.txt')
ids = []

rawids.each do |id|
  ids.push(id.split(''))
end

ids.each do |v|
  ids.each do |x|
    diff = v-x;
    if(diff === [])
      #same
      next
    end

    if(diff.length > 1)
     # too many diffs
     next
    end

    if(diff.length == 1)
      # just one diff
      count = 0
      lastindex = 0

      for i in 0..x.length
        if x[i] != v[i]
          lastindex = i
          count += 1
        end
      end

      if(count == 1)
        puts x.join('')
        puts v.join('')
        x.delete_at(lastindex)
        puts x.join('')
      end
    end
  end
end